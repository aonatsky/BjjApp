using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Repositories;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace TRNMNT.Web.Controllers
{
    public abstract class CRUDController<T> : BaseController where T : class
    {
        IRepository<T> repository;
        public CRUDController(ILogger logger, IRepository<T> repository, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(logger, httpContextAccessor, userService)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<T>> Get()
        {
            try
            {
                return await repository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new List<T>();
            }

        }

        [HttpGet("{entityID}")]
        public async Task<T> Get(string entityID)
        {
            try
            {
                return await repository.GetByIDAsync(entityID);
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
                repository.Add(entity);
                await repository.SaveAsync(false);
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
                repository.Update(entity);
                await repository.SaveAsync(false);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                base.HandleException(e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        [Authorize, HttpDelete]
        public async Task Delete([FromBody]T entity)
        {
            try
            {
                repository.Delete(entity);
                await repository.SaveAsync(false);
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
                repository.Delete<Guid>(Guid.Parse(entityID));
                await repository.SaveAsync(false);
                Response.StatusCode = (int)HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                base.HandleException(e);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}