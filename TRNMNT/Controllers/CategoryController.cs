using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            IUserService userService) : base(logger, repository, httpContextAccessor, userService)
        {
            this.categoryService = categoryService;
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


