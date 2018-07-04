using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TRNMNT.Core.Authentication;
using TRNMNT.Core.Configurations;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Core.Settings;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Dependencies

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityErrorDescriber _identityErrorDescriber;
        private readonly IAuthConfiguration _authConfiguration;
        private readonly IConfiguration _configuration;

        #endregion

        #region .ctor

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IAuthConfiguration authConfiguration, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _identityErrorDescriber = new IdentityErrorDescriber();
            _authConfiguration = authConfiguration;
            _configuration = configuration;
        }

        #endregion

        #region Public methods

        public async Task AddSampleUserAsync()
        {
            await AddSampleRolesAsync();
            var existUser = await _userManager.FindByNameAsync("admin");
            if (existUser != null)
            {
                //await _userManager.DeleteAsync(existUser);
            }

            if (existUser == null)
            {
                var identity = await _userManager.CreateAsync(new User
                {
                    UserName = "admin",
                        FirstName = "Ivan",
                        LastName = "Drago",
                        Email = "Ivan.drago@trnmnt.com"
                }, "1");
                existUser = await _userManager.FindByNameAsync("admin");
                var result = await _userManager.AddClaimAsync(existUser, new Claim(ClaimTypes.Role, Roles.Owner));
            }
        }

        public async Task<AuthTokenResult> GetTokenAsync(string login, string password)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(login, password, false, false);
            if (loginResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(login);
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

        public async Task<AuthTokenResult> GetTokenAsync()
        {
            await AddSampleUserAsync();
            var user = await _userManager.FindByNameAsync("admin");

            if (user != null)
            {
                var requestAt = DateTime.Now;

                var token = GenerateTokenAsync(user);
                return await GetTokensAsync(user);
            }
            return null;
        }

        public async Task<UserRegistrationResult> CreateParticipantUserAsync(string email, string password)
        {
            return await CreateUserAsync(new UserCredentialsModel()
            {
                Email = email,
                    Password = password
            });
        }
        #endregion

        #region Private Methods
        public async Task<UserRegistrationResult> CreateUserAsync(UserCredentialsModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var identityResult = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, model.Role));

            if (identityResult.Succeeded)
            {
                return new UserRegistrationResult(true);
            }
            return new UserRegistrationResult(false, identityResult.Errors.FirstOrDefault()?.Description);
        }

        private async Task AddSampleRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Owner))
            {
                var newRole = new IdentityRole(Roles.Owner);
                await _roleManager.CreateAsync(newRole);
            }
        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity(
                GetTokenClaims(user).Union(await _userManager.GetClaimsAsync(user))
            );

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
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),

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

        public async Task<SocialLoginResult> FacebookLogin(string fbToken)
        {
            var client = new HttpClient();
            // 1.generate an app access token
            var appAccessTokenResponse =
                await client.GetStringAsync(string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials", _configuration["Facebook:AppId"], _configuration["Facebook:AppSecret"]));
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse =
                await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={fbToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new Exception("Invalid facebook data");
            }

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={fbToken}");
            var fbUser = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);
            var user = await _userManager.FindByEmailAsync(fbUser.Email);

            var result = new SocialLoginResult()
            {
                UserData = new UserRegistrationModel()
                {
                FirstName = fbUser.FirstName,
                LastName = fbUser.LastName,
                Email = fbUser.Email,
                }
            };
            if (user != null)
            {
                result.IsExistingUser = true;
                result.AuthTokenResult = await GetTokensAsync(user);
            }
            if (!String.IsNullOrEmpty(fbUser.DateOfBirthString))
            {
                result.UserData.DateOfBirth = DateTime.Parse(fbUser.DateOfBirthString);
            }
            return result;
        }
        #endregion
    }

}