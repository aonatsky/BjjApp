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

        [Authorize, HttpGet]
        public async Task<List<WeightDivision>> Get(string categoryId)
        {
            try
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return await weightDivisionService.GetWeightDivisionsByCategoryId(Guid.Parse(categoryId));
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
                return null;
            }
        }

        public override IQueryable<WeightDivision> ProcessQuery(string key, string value, IQueryable<WeightDivision> query)
        {
            throw new NotImplementedException();
        }
    }

}
