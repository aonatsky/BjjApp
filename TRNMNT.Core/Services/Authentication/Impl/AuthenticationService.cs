using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Web.Core.Settings;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Web.Core.Services.Authentication.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IdentityErrorDescriber identityErrorDescriber = new IdentityErrorDescriber();

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }

        #region Public methods
        public async Task<string> GetTokenAsync(string login, string password)
        {
            await AddSampleUserAsync();
            var loginResult = await signInManager.PasswordSignInAsync(login, password, false, false);
            if (loginResult.Succeeded)
            {
                var user = await userManager.FindByNameAsync(login);
                var token = GenerateTokenAsync(user);
                return await token;
            }
            return String.Empty;
        }

        public async Task<string> GetTokenAsync()
        {
            await AddSampleUserAsync();
            var user = await userManager.FindByNameAsync("admin");


            if (user != null)
            {
                var requestAt = DateTime.Now;

                var token = GenerateTokenAsync(user);
                return await token;
            }
            else
            {
                return "";
            }
        }

        public async Task<UserRegistrationResult> CreateParticipantUserAsync(string email, string password)
        {
            return await CreateUserAsync(email, password, Roles.ROLE_PARTICIPANT);
        }

        public async Task<UserRegistrationResult> CreateOwnerUserAsync(string email, string password)
        {
            return await CreateUserAsync(email, password, Roles.ROLE_OWNER);
        }



        #endregion

        #region Private Methods
        private async Task<UserRegistrationResult> CreateUserAsync(string email, string password, string roleClaim)
        {
            var user = new User()
            {
                Email = email,
                UserName = email
            };
            var identityResult = await userManager.CreateAsync(user, password);
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleClaim));

            if (identityResult.Succeeded)
            {
                return new UserRegistrationResult(true);
            }
            else
            {
                return new UserRegistrationResult(false, identityResult.Errors.FirstOrDefault().Description);
            }



        }

        private async Task AddSampleUserAsync()
        {
            await AddSampleRolesAsync();
            var existUser = await userManager.FindByNameAsync("admin");
            if (existUser != null)
            {
                //await _userManager.DeleteAsync(existUser);
            }

            if (existUser == null)
            {
                var identity = await userManager.CreateAsync(new User
                {
                    UserName = "admin",
                    FirstName = "Ivan",
                    LastName = "Drago",
                    Email = "Ivan.drago@trnmnt.com"
                }, "1");
                existUser = await userManager.FindByNameAsync("admin");
                var result = await userManager.AddClaimAsync(existUser, new Claim(ClaimTypes.Role, Roles.ROLE_OWNER));

            }

        }

        private async Task AddSampleRolesAsync()
        {
            if (!(await roleManager.RoleExistsAsync(Roles.ROLE_OWNER)))
            {
                IdentityRole newRole = new IdentityRole(Roles.ROLE_OWNER);
                await roleManager.CreateAsync(newRole);
            }
        }

        private async Task<string> GenerateTokenAsync(User user)
        {

            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                GetTokenClaims(user).Union(await userManager.GetClaimsAsync(user))
            );

            var expiresIn = DateTime.Now + TimeSpan.FromMinutes(TokenAuthOptions.Lifetime);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOptions.Issuer,
                Audience = TokenAuthOptions.Audience,
                SigningCredentials = new SigningCredentials(TokenAuthOptions.GetKey(), SecurityAlgorithms.HmacSha256),
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

        #endregion

    }
}
