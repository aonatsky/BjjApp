using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;
using TRNMNT.Core.Services;

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class TournamentSettings : BaseController
    {
        public TournamentSettings(ILogger<HomeController> logger, 
        IRepository<WeightDivision> weightDivisionrepository,
        IRepository<Category> categoryRepository
        ):base(logger)
        {
                        
        }

        [HttpGet("[action]")]
        public IEnumerable<Category> Categories()
        {
            return new List<Category>(){
                new Category(){CategoryID = 0, Name = "Kids"},
                new Category(){CategoryID = 1, Name = "Junior"},
                new Category(){CategoryID = 2, Name = "Adult - White"},
                new Category(){CategoryID = 3, Name = "Adult - Blue"}
            };
        }

    }
}
