using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using TRNMNT.Web.Core.Services.Authentication;
using TRNMNT.Web.Core.Model;
using System;
using Microsoft.AspNetCore.Authorization;
using TRNMNT.Core.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService authenticationSerivce;

        public AuthController(ILogger<AuthController> logger, IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(logger, httpContextAccessor,userService)
        {
            authenticationSerivce = authenticationService;
        }


        [AllowAnonymous, HttpPost("[action]")]
        public async Task GetToken([FromBody] UserCredentialsModel credentials)
        {
            try
            {
                //var token = await _authenticationSerivce.GetToken(credentials.Username, credentials.Password);
                var token = await authenticationSerivce.GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    var response = new
                    {
                        id_token = token,
                    };
                    await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));

                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task Register([FromBody] UserCredentialsModel credentials)
        {
            try
            {
                await authenticationSerivce.CreateAccountAsync(credentials.Username, credentials.Password);
                Response.StatusCode = ((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Response.StatusCode = ((int)HttpStatusCode.InternalServerError);
                HandleException(ex);
            }

        }



    }


}
