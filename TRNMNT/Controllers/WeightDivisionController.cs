using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class WeightDivisionController : CRUDController<WeightDivision>
    {
        public WeightDivisionController(ILogger<WeightDivisionController> logger, IRepository<WeightDivision> repository, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(logger, repository, httpContextAccessor, userService)
        {
        }
    }

}
