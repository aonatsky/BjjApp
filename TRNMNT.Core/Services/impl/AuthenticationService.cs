using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TRNMNT.Core.Configurations;
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

        #endregion

        #region .ctor

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IAuthConfiguration authConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _identityErrorDescriber = new IdentityErrorDescriber();
            _authConfiguration = authConfiguration;
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
                var user = await _userManager.FindByNameAsync(login);
                return await GetTokens(user);
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
                return await GetTokens(user);
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
                return await GetTokens(user);
            }
            return null;
        }

        public async Task<UserRegistrationResult> CreateParticipantUserAsync(string email, string password)
        {
            return await CreateUserAsync(email, password, Roles.Participant);
        }

        public async Task<UserRegistrationResult> CreateOwnerUserAsync(string email, string password)
        {
            return await CreateUserAsync(email, password, Roles.Owner);
        }

        #endregion

        #region Private Methods

        private async Task<UserRegistrationResult> CreateUserAsync(string email, string password, string roleClaim)
        {
            var user = new User
            {
                Email = email,
                UserName = email,
                FirstName = "",
                LastName = ""
            };
            var identityResult = await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleClaim));

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
            return new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),    
                //new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),

                };
        }

        private List<Claim> GetRefreshTokenClaims(User user)
        {
            return new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
        }

        private async Task<AuthTokenResult> GetTokens(User user)
        {
            return new AuthTokenResult
            {
                AccessToken = await GenerateTokenAsync(user),
                RefreshToken = GenerateRefreshToken(user)
            };
        }

        #endregion
    }
}
