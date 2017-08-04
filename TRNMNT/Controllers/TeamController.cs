using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Entities;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TRNMNT.Data.Repositories;
using System.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : CRUDController<Team>
    {
        IHttpContextAccessor httpContextAccessor;

        public TeamController(IEventService eventService, ILogger<TeamController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService, IRepository<Team> repository) 
            : base(logger, repository, httpContextAccessor, userService)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override IQueryable<Team> ModifyQuery(string key, string value, IQueryable<Team> query)
        {
            throw new NotImplementedException();
        }
    }
}

