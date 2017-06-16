using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TRNMNT.Data.Entities;

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

        public AuthController(ILogger logger, UserManager<User> userManager,
            SignInManager<User> signInManager) : base(logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

       
    }
}
