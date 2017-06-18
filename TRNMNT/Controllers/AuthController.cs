using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TRNMNT.Data.Entities;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Newtonsoft.Json;
using TRNMNT.Web.Core.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(ILogger<AuthController> logger, UserManager<User> userManager,
            SignInManager<User> signInManager) : base(logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }


        [HttpGet("[action]")]
        public async Task GetToken()
        {
            var existUser = await _userManager.FindByNameAsync("admin");
            if (existUser == null)
            {
                await _userManager.CreateAsync(new User
                {
                    UserName = "admin"
                });
                existUser = await _userManager.FindByNameAsync("admin");
            }

            if (existUser != null)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TimeSpan.FromMinutes(TokenAuthOption.LIFETIME);
                var token = GenerateToken(existUser, expiresIn);

                Response.StatusCode = (int)HttpStatusCode.OK;
                var response = new
                {
                    accessToken = token,
                    userName = existUser.UserName
                };
                await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));

            }
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
                Issuer = TokenAuthOption.ISSUER,
                Audience = TokenAuthOption.AUDIENCE,
                SigningCredentials = new SigningCredentials(TokenAuthOption.GetKey(), SecurityAlgorithms.HmacSha256),
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
    }
}
;