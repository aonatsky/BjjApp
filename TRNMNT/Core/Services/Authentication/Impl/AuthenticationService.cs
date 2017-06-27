using Microsoft.AspNetCore.Identity;
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

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signinManager)
        {
            _userManager = userManager;
            _signInManager = signinManager;

        }

        public async Task<string> GetToken()
        {
            var existUser = await _userManager.FindByNameAsync("admin");
            if (existUser == null)
            {
                await _userManager.CreateAsync(new User
                {
                    UserName = "admin",
                    FirstName = "Ivan",
                    LastName= "Drago"
                },"1");
                existUser = await _userManager.FindByNameAsync("admin");
            }



            if (existUser != null)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TimeSpan.FromMinutes(TokenAuthOptions.LIFETIME);
                var token = GenerateToken(existUser, expiresIn);
                return token;
            }
            else
            {
                return "";
            }
        }
            

        public Task<string> GetToken(string login, string password)
        {
            throw new NotImplementedException();
        }



        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.UserName, "TokenAuth"),
                new[] {
                    new Claim("ID", user.Id.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOptions.ISSUER,
                Audience = TokenAuthOptions.AUDIENCE,
                SigningCredentials = new SigningCredentials(TokenAuthOptions.GetKey(), SecurityAlgorithms.HmacSha256),
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);

        }
    }
}
