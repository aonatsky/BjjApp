using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class WeightDivisionController : CRUDController<WeightDivision>
    {
        public WeightDivisionController(ILogger<WeightDivisionController> logger, IRepository<WeightDivision> repository) : base(logger, repository)
        {
        }
    }

}
