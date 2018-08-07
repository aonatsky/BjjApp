using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TRNMNT.Core.Authentication;
using TRNMNT.Core.Configurations;
using TRNMNT.Core.Const;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Model.User;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        #region Dependencies

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityErrorDescriber _identityErrorDescriber;
        private readonly IAuthConfiguration _authConfiguration;
        private readonly IConfiguration _configuration;

        #endregion

        #region .ctor

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IAuthConfiguration authConfiguration, IConfiguration configuration, ILogger<AuthenticationService> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _identityErrorDescriber = new IdentityErrorDescriber();
            _authConfiguration = authConfiguration;
            _configuration = configuration;
        }

        #endregion

        #region Public methods

        public async Task<User> GetUser(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<AuthTokenResult> GetTokenAsync(string login, string password)
        {

            var loginResult = await _signInManager.PasswordSignInAsync(login, password, false, false);

            if (loginResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(login);
                if (user == null)
                {
                    return null;
                }
                return await GetTokensAsync(user);
            }
            return null;
        }

        public async Task<AuthTokenResult> UpdateTokenAsync(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(refreshToken);
            var userIdClaim = data.Payload.FirstOrDefault(x => x.Key.Equals("nameid", StringComparison.CurrentCulture));
            if (userIdClaim.Value != null)
            {
                var user = await _userManager.FindByIdAsync(userIdClaim.Value.ToString());
                return await GetTokensAsync(user);
            }
            return null;
        }

        public async Task<UserRegistrationResult> CreateParticipantUserAsync(UserRegistrationModel model)
        {
            return await CreateUserAsync(model, Roles.Participant);
        }
        #endregion

        public async Task UpdateUserAsync(UserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                throw new BusinessException("ERROR.USER_NOT_FOUND");
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth.Value;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
        }
        public async Task<UserRegistrationResult> CreateUserAsync(UserRegistrationModel model, string role)
        {
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreateTS = DateTime.UtcNow,
                UserName = model.Email,
                IsActive = true
            };
            return await CreateUserWithRoleAsync(user, role, model.Password);
        }

        public async Task<AuthTokenResult> FacebookLogin(string fbToken)
        {
            var client = new HttpClient();
            // 1.generate an app access token
            // _logger.LogInformation($"Getting access token. creds {_configuration["FACEBOOK_APPID"]}, {_configuration["FACEBOOK_APPSECRET"]}");
            var appAccessTokenResponse =
                await client.GetStringAsync(string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials", _configuration["FACEBOOK_APPID"], _configuration["FACEBOOK_APPSECRET"]));
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            // _logger.LogInformation("validation access token");
            var userAccessTokenValidationResponse =
                await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={fbToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new BusinessException("Invalid facebook data");
            }

            // 3. we've got a valid token so we can request user data from fb
            _logger.LogInformation("Getting user info");
            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={fbToken}");
            var fbUser = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);
            var user = await _userManager.FindByEmailAsync(fbUser.Email);
            if (user == null)
            {
                user = new User()
                {
                Email = fbUser.Email,
                FirstName = fbUser.FirstName,
                LastName = fbUser.LastName,
                FacebookId = fbUser.Id,
                UserName = fbUser.Email,
                PictureUrl = fbUser.Picture.Data.Url
                };
                await CreateUserWithRoleAsync(user, Roles.Participant, "facebookPass1");
            }

            return await GetTokensAsync(user);
        }

        public async Task ChangesPasswordAsync(string oldPassword, string newPassword, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException("ERROR.USER_NOT_FOUND");
            }
            var changeResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changeResult.Succeeded)
            {
                throw new BusinessException("ERROR.CHANGE_PASSWORD.OLD_PASSWORD_IS_INVALID");
            };
        }

        public async Task SetPasswordAsync(string password, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException("ERROR.USER_NOT_FOUND");
            }
            var setPasswordResult = await _userManager.AddPasswordAsync(user, password);
            if (setPasswordResult.Succeeded)
            {
                throw new BusinessException("ERROR.PASSWORD.OLD_PASSWORD_IS_INVALID");
            }
        }

        #region Private Methods

        private async Task<UserRegistrationResult> CreateUserWithRoleAsync(User user, string role, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));

            if (identityResult.Succeeded)
            {
                return new UserRegistrationResult(true);
            }
            return new UserRegistrationResult(false, identityResult.Errors.FirstOrDefault()?.Description);
        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity(
                GetTokenClaims(user).Union(await _userManager.GetClaimsAsync(user))
            );
            _logger.LogDebug($"Configuration is {_authConfiguration != null}");
            _logger.LogDebug($"Configuration key is {_authConfiguration.Key}");
            var expiresIn = DateTime.Now + TimeSpan.FromMinutes(_authConfiguration.AccessTokenLifetime);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authConfiguration.Issuer,
                    Audience = _authConfiguration.Audience,
                    SigningCredentials = new SigningCredentials(_authConfiguration.Key, SecurityAlgorithms.HmacSha256),
                    Subject = identity,
                    Expires = expiresIn
            });
            return handler.WriteToken(securityToken);
        }

        private string GenerateRefreshToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity(GetRefreshTokenClaims(user));
            var expiresIn = DateTime.Now + TimeSpan.FromMinutes(_authConfiguration.RefreshTokenLifetime);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authConfiguration.Issuer,
                    Audience = _authConfiguration.Audience,
                    SigningCredentials = new SigningCredentials(_authConfiguration.Key, SecurityAlgorithms.HmacSha256),
                    Subject = identity,
                    Expires = expiresIn
            });
            return handler.WriteToken(securityToken);
        }

        private List<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(JWTClaimNames.UserId, user.Id),
                new Claim(JWTClaimNames.FirstName, user.FirstName),
                new Claim(JWTClaimNames.LastName, user.LastName),
                new Claim(JWTClaimNames.Email, user.Email),
                new Claim(JWTClaimNames.DateOfBirth, user.DateOfBirth.ToString())
            };
        }

        private List<Claim> GetRefreshTokenClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
        }

        private async Task<AuthTokenResult> GetTokensAsync(User user)
        {
            return new AuthTokenResult()
            {
                IdToken = await GenerateTokenAsync(user),
                    RefreshToken = GenerateRefreshToken(user)
            };
        }

        #endregion

    }

}