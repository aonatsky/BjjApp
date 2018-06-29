using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        #region Dependencies

        private readonly IAuthenticationService _authenticationSerivce;

        #endregion

        #region .ctor

        public AuthController(
            ILogger<AuthController> logger,
            IAuthenticationService authenticationService,
            IUserService userService,
            IEventService eventService,
            IAppDbContext context
        ) : base(logger, userService, eventService, context)
        {
            _authenticationSerivce = authenticationService;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous, HttpPost("[action]")]
        public async Task GetToken([FromBody] UserCredentialsModel credentials)
        {
            try
            {
                //var token = await _authenticationSerivce.GetToken(credentials.Email, credentials.Password);
                var tokenResult = await _authenticationSerivce.GetTokenAsync(credentials.Email, credentials.Password);
                if (tokenResult != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    var response = new
                    {
                        id_token = tokenResult.AccessToken,
                        refresh_token = tokenResult.RefreshToken
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
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task UpdateToken([FromBody] RefreshTokenModel refreshTokenModel)
        {
            try
            {
                var tokenResult = await _authenticationSerivce.UpdateTokenAsync(refreshTokenModel.RefreshToken);
                if (tokenResult != null)
                {
                    var response = new
                    {
                        id_token = tokenResult.AccessToken,
                        refresh_token = tokenResult.RefreshToken
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
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task Register([FromBody] UserCredentialsModel credentials)
        {
            try
            {
                if (Request.Headers["password"] == "pizdecpassword")
                {
                    await _authenticationSerivce.CreateUserAsync(credentials);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden; ;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(ex);
            }
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task FacebookLogin([FromBody]FacebookAuthViewModel model)
        {
            try
            {
                var tokenResult = await _authenticationSerivce.FacebookLogin(model.Token);
                if (tokenResult != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    var response = new
                    {
                        id_token = tokenResult.AccessToken,
                        refresh_token = tokenResult.RefreshToken
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
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(ex);
            }
        }

        #endregion
    }
}
