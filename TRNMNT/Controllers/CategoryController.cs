using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Services;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        #region Dependencies

        private readonly ICategoryService _categoryService;

        #endregion

        #region .ctor

        public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryService categoryService,
            IRepository<Category> repository,
            IHttpContextAccessor httpContextAccessor,
            IEventService eventService,
            IUserService userService,
            IAppDbContext context) : base(logger, userService, eventService, context)
        {
            _categoryService = categoryService;
        }

        #endregion

        #region Public Methods

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategoriesForEvent()
        {
            try
            {
                var eventId = GetEventId();
                if (eventId != null)
                {
                    var data = await _categoryService.GetCategoriesByEventIdAsync(eventId.Value);
                    return Ok(JsonConvert.SerializeObject(data, JsonSerializerSettings));
                }
                return NotFound();
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }

        #endregion
    }
}


