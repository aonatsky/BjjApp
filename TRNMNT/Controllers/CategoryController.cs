using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    public class CategoryController : CRUDController<Category>
    {
        private ICategoryService categoryService;

        public CategoryController(
            ILogger<CategoryController> logger, 
            ICategoryService categoryService, 
            IRepository<Category> repository, 
            IHttpContextAccessor httpContextAccessor, 
            IEventService eventService,
            IUserService userService) : base(logger, repository, userService, eventService)
        {
            this.categoryService = categoryService;
        }


        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetCategoriesForEvent(string eventId)
        {
            try
            {
                var user = await GetUserAsync();
                var data = await categoryService.GetCategoriesByEventIdAsync(Guid.Parse(eventId));
                return Ok(JsonConvert.SerializeObject(data, jsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }



        public override IQueryable<Category> ModifyQuery(string key, string value, IQueryable<Category> query)
        {
            switch (key)
            {
                case "eventId":
                    {
                        query = query.Where(c => c.EventId == Guid.Parse(value));
                        break;
                    } 
            }
            return query;
        }
    }
}


