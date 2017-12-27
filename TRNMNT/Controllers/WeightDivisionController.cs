using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Services;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class WeightDivisionController : CRUDController<WeightDivision>
    {
        private IWeightDivisionService weightDivisionService;

        public WeightDivisionController(ILogger<WeightDivisionController> logger,
            IWeightDivisionService weightDivisionService,
            IRepository<WeightDivision> repository,
            IUserService userService,
            IEventService eventService,
            IAppDbContext context)
            : base(logger, repository, userService, eventService, context)
        {
            this.weightDivisionService = weightDivisionService;
        }


        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetWeightDivisionsByCategory(Guid categoryId)
        {
            try
            {
                var data = await weightDivisionService.GetWeightDivisionsByCategoryIdAsync(categoryId);
                return Ok(JsonConvert.SerializeObject(data, jsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }

        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetWeightDivisionsByEvent(Guid eventId)
        {
            try
            {
                var data = await weightDivisionService.GetWeightDivisionsByEventIdAsync(eventId);
                return Ok(JsonConvert.SerializeObject(data, jsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }



        public override IQueryable<WeightDivision> ModifyQuery(string key, string value, IQueryable<WeightDivision> query)
        {
            switch (key)
            {
                case "categoryId":
                    {
                        query = query.Where(w => w.CategoryId == Guid.Parse(value));
                        break;
                    }
            }

            return query;
        }
    }

}
