﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.User;
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
        private readonly ILogger<AuthController> logger;
        #region Dependencies

        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region .ctor

        public AuthController(
            ILogger<AuthController> logger,
            IAuthenticationService authenticationService,
            IUserService userService,
            IEventService eventService,
            IAppDbContext context,
            IConfiguration configuration
        ) : base(logger, userService, eventService, context, configuration)
        {
            this.logger = logger;
            _authenticationService = authenticationService;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> GetToken([FromBody] UserCredentialsModel credentials)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var tokenResult = await _authenticationService.GetTokenAsync(credentials.Email, credentials.Password);
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
                var tokenResult = await _authenticationService.UpdateTokenAsync(refreshTokenModel.RefreshToken);
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
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginModel model)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var authTokenresult = await _authenticationService.FacebookLogin(model.Token);
                if (authTokenresult != null)
                {
                    return Success(authTokenresult);
                }
                else
                {
                    return (null, HttpStatusCode.Unauthorized);
                }
            });
        }
        #endregion
    }
}