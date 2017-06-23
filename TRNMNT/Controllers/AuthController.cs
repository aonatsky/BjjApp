using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Http;
using TRNMNT.Web.Core.Services.Authentication;
using TRNMNT.Web.Core.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationSerivce;

        public AuthController(ILogger<AuthController> logger, IAuthenticationService authenticationService) : base(logger)
        {
            _authenticationSerivce = authenticationService;
        }


        [HttpPost("[action]")]
        public async Task GetToken([FromBody] UserCredentialsModel credentials)
        {

                var token = await _authenticationSerivce.GetToken();
                Response.StatusCode = (int)HttpStatusCode.OK;
                var response = new
                {
                    id_token = token,
                };
                await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));

            }
        }

       
    }
