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

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class WeightDivisionController : CRUDController<WeightDivision>
    {
        private IWeightDivisionService weightDivisionService;

        public WeightDivisionController(ILogger<WeightDivisionController> logger, IWeightDivisionService weightDivisionService, IRepository<WeightDivision> repository, IUserService userService, IEventService eventService)
            : base(logger, repository, userService, eventService)
        {
            this.weightDivisionService = weightDivisionService;
        }


        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetWeightDivisionsByCategory(string categoryId)
        {
            try
            {
                var data = await weightDivisionService.GetWeightDivisionsByCategoryIdAsync(Guid.Parse(categoryId));
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
