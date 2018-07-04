﻿using System;
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
        public async Task<IActionResult> GetToken([FromBody] UserCredentialsModel credentials)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var tokenResult = await _authenticationSerivce.GetTokenAsync(credentials.Email, credentials.Password);
                if (tokenResult != null)
                {
                    Response.StatusCode = (int) HttpStatusCode.OK;
                    var response = new
                    {
                        idToken = tokenResult.IdToken,
                        refreshToken = tokenResult.RefreshToken
                    };
                    return Success(response);
                }
                else
                {
                    return (null, HttpStatusCode.Unauthorized);
                };
            });
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> UpdateToken([FromBody] RefreshTokenModel refreshTokenModel)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var tokenResult = await _authenticationSerivce.UpdateTokenAsync(refreshTokenModel.RefreshToken);
                if (tokenResult != null)
                {
                    return Success(tokenResult);
                }
                else
                {
                    return (null, HttpStatusCode.Unauthorized);
                }
            });
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserCredentialsModel credentials)
        {
            return await HandleRequestAsync(async() =>
            {
                if (Request.Headers["password"] == "pizdecpassword")
                {
                    await _authenticationSerivce.CreateUserAsync(credentials);
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.Forbidden;;
                }
            });
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginModel model)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var socialLoginResult = await _authenticationSerivce.FacebookLogin(model.Token);
                if (socialLoginResult != null)
                {
                    return Success(socialLoginResult);
                }
                else
                {
                    return (null,HttpStatusCode.Unauthorized);
                }
            });
        }
        #endregion
    }
}