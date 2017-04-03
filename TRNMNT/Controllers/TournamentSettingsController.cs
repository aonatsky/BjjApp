using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;
using TRNMNT.Core.Services;

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class TournamentSettingsController : BaseController
    {
        
        ILogger<TournamentSettingsController> logger;
        ITournamentSettingsService tournamentSettingsService;
        
        public TournamentSettingsController(ILogger<TournamentSettingsController> logger,
        ITournamentSettingsService tournamentSettingsService
        ) : base(logger)
        {
            this.logger =logger;
            this.tournamentSettingsService = tournamentSettingsService;
        }

        [HttpGet("[action]")]
        public IEnumerable<Category> Categories()
        {
            var response = new List<Category>(){
                new Category(){CategoryID = 0, Name = "Kids"},
                new Category(){CategoryID = 1, Name = "Junior"},
                new Category(){CategoryID = 2, Name = "Adult - White"},
                new Category(){CategoryID = 3, Name = "Adult - Blue"}
                        };
            return response;
        }

    }
}
