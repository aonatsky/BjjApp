using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Data.Repositories;

namespace TRNMNT.Controllers
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
            return repository.GetAll().ToList();
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
                Response.StatusCode = 500;
                return e.ToString();
            }
            return "OK";
        }

        [HttpDelete]
        public String Delete(T entity)
        {
            Response.StatusCode = 200;

            try
            {
                repository.Delete(entity);
                repository.Save(false);

            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return e.ToString();
            }
            return "OK";
        }
    }
}