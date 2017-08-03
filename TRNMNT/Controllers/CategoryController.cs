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


        //[Authorize, HttpGet]
        //public async Task<IEnumerable<Category>> Get()
        //{
        //    try
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.OK;
        //        return await categoryService.GetCategoriesByEventId(Guid.Parse("1"));
        //    }
        //    catch (Exception e)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        HandleException(e);
        //        return null;
        //    }
        //}

        public override IQueryable<Category> ProcessQuery(string key, string value, IQueryable<Category> query)
        {
            var newQuery = query;
            switch (key)
            {
                case "eventId":
                    {
                        newQuery = newQuery.Where(c => c.EventId == Guid.Parse(value));
                        break;
                    } 
            }
            return newQuery;
        }
    }
}


