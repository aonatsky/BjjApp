using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Web.Controllers
{
    public abstract class CRUDController<T> : BaseController where T : class
    {
        #region Dependencies

        protected readonly IRepository<T> Repository;

        #endregion

        #region .ctor

        protected CRUDController(ILogger logger,
            IRepository<T> repository,
            IUserService userService,
            IEventService eventService,
            IConfiguration configuration,
            IAppDbContext context) : base(logger, userService, eventService, context, configuration)
        {
            Repository = repository;
        }

        #endregion

        #region Public Methods


        [Authorize, HttpGet]
        public async Task<IEnumerable<T>> Get()
        {
            try
            {
                var query = Repository.GetAll();
                var queryParams = HttpContext.Request.Query.ToList();
                if (queryParams.Any())
                {
                    foreach (var param in queryParams)
                    {
                        query = ModifyQuery(param.Key, param.Value, query);
                    }
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new List<T>();
            }

        }

        public abstract IQueryable<T> ModifyQuery(string key, string value, IQueryable<T> query);

        [HttpGet("{entityID}")]
        public async Task<T> Get(string entityID)
        {
            try
            {
                return await Repository.GetByIDAsync(entityID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return null;
            }

        }

        [Authorize, HttpPost]
        public async Task Post([FromBody] T entity)
        {
            Response.StatusCode = 200;

            try
            {
                Repository.Add(entity);
                //await repository.SaveAsync(false);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                HandleException(e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            }

        }
        [Authorize, HttpPut]
        public async Task Put([FromBody] T entity)
        {
            try
            {
                Repository.Update(entity);
                //await repository.SaveAsync(false);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                HandleException(e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        [Authorize, HttpDelete]
        public async Task Delete([FromBody]T entity)
        {
            try
            {
                Repository.Delete(entity);
                //await repository.SaveAsync(false);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                HandleException(e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        [Authorize, HttpDelete("{entityID}")]
        public async Task Delete(string entityID)
        {
            try
            {
                Repository.Delete<Guid>(Guid.Parse(entityID));
                //await repository.SaveAsync(false);
                Response.StatusCode = (int)HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                HandleException(e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        #endregion
    }
}