using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.User;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        #region .ctor

        public UserController(ILogger<UserController> logger, IUserService userService, IEventService eventService, IAppDbContext context)
            : base(logger, userService, eventService, context) { }

        #endregion

        #region Public Methods

        [HttpPost("[action]")]
        public async Task<IActionResult> SecretCreation([FromBody]SecretUserCreationModel secretUserCreationModel)
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

        #endregion
    }
}
