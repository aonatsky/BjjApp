using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Web.Controllers
{
    public abstract class CRUDController<T> : BaseController where T : class
    {
        IRepository<T> repository;
        public CRUDController(ILogger logger, IRepository<T> repository) : base(logger)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            try
            {
                return repository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                Response.StatusCode = 500;
                return new List<T>();
            }

        }

        [HttpPost]
        public string Post([FromBody] T entity)
        {
            Response.StatusCode = 200;

            try
            {
                repository.Add(entity);
                repository.Save(false);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return e.ToString();
            }
            return "OK";
        }
        [HttpPut]
        public string Put([FromBody] T entity)
        {
            Response.StatusCode = 200;

            try
            {

                repository.Update(entity);
                repository.Save(false);

            }
            catch (Exception e)
            {
                base.HandleException(e);
                Response.StatusCode = 500;
            }
            return "OK";
        }

        [HttpDelete]
        public String Delete([FromBody]T entity)
        {
            Response.StatusCode = 200;

            try
            {
                repository.Delete(entity);
                repository.Save(false);

            }
            catch (Exception e)
            {
                base.HandleException(e);
                Response.StatusCode = 500;
            }
            return "OK";
        }

        [HttpDelete("{entityID}")]
        public String Delete(string entityID)
        {
            Response.StatusCode = 200;

            try
            {
                repository.Delete<Guid>(Guid.Parse(entityID));
                repository.Save(false);

            }
            catch (Exception e)
            {
                base.HandleException(e);
                Response.StatusCode = 500;
                
            }
            return "OK";
        }
    }
}