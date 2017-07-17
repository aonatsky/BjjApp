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

namespace TRNMNT.Web.Core.Services.Authentication.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signinManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signinManager;
            _roleManager = roleManager;

        }

        public async Task<string> GetTokenAsync()
        {
            await AddSampleUserAsync();
            var user = await _userManager.FindByNameAsync("admin");


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

        private async Task AddSampleUserAsync()
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
                var result = await _userManager.AddClaimAsync(existUser, new Claim(ClaimTypes.Role, Roles.ROLE_OWNER));
                
            }

        }


        private async Task AddSampleRolesAsync()
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.ROLE_OWNER)))
            {
                IdentityRole newRole = new IdentityRole(Roles.ROLE_OWNER);
                await _roleManager.CreateAsync(newRole);
            }
        }

        public async Task<string> GetTokenAsync(string login, string password)
        {
            await AddSampleUserAsync();
            var loginResult = await _signInManager.PasswordSignInAsync(login, password, false, false);
            if (loginResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login);
                var token = GenerateTokenAsync(user);
                return await token;
            }
            return String.Empty;
        }



        private async Task<string> GenerateTokenAsync(User user)
        {

            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                GetTokenClaims(user).Union(await _userManager.GetClaimsAsync(user))
            );

            var expiresIn = DateTime.Now + TimeSpan.FromMinutes(TokenAuthOptions.LIFETIME);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOptions.ISSUER,
                Audience = TokenAuthOptions.AUDIENCE,
                SigningCredentials = new SigningCredentials(TokenAuthOptions.GetKey(), SecurityAlgorithms.HmacSha256),
                Subject = identity,
                Expires = expiresIn
            });
            return handler.WriteToken(securityToken);

        }


        private List<Claim> GetTokenClaims(User user)
        {
            return new List<Claim> {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimNames.FIRST_NAME, user.FirstName),
                    new Claim(ClaimNames.LAST_NAME, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),

                };
        }
    }
}
