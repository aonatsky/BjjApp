using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private IWeightDivisionService weightDivisionService;

        public WeightDivisionController(ILogger<WeightDivisionController> logger, IWeightDivisionService weightDivisionService, IRepository<WeightDivision> repository, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(logger, repository, httpContextAccessor, userService)
        {
            this.weightDivisionService = weightDivisionService;
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
