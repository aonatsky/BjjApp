using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Services;
using TRNMNT.Data.Context;
using TRNMNT.Web.Core.Model;
using TRNMNT.Web.Core.Services.Authentication;

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


        [AllowAnonymous, HttpPost("[action]")]
        public async Task GetToken([FromBody] UserCredentialsModel credentials)
        {
            try
            {
                //var token = await _authenticationSerivce.GetToken(credentials.Username, credentials.Password);
                var token = await _authenticationSerivce.GetTokenAsync(credentials.Username, credentials.Password);
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
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task Register([FromBody] UserCredentialsModel credentials)
        {
            try
            {
                await _authenticationSerivce.CreateParticipantUserAsync(credentials.Username, credentials.Password);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(ex);
            }

        }



    }


}
