using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TRNMNT.Data.Entities;
using TRNMNT.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class EventDetailsController : Controller
    {
        private readonly IWeightDivisionService _WeightDivisionService;
        public EventDetailsController(IWeightDivisionService WeightDivisionService)
        {
            _WeightDivisionService = WeightDivisionService;
        }
        // GET api/values
        [HttpGet("[action]")]
        public IEnumerable<WeightDivision> GetWeightDivisiones()
        {
            var WeightDivisiones = _WeightDivisionService.GetWeghtClasses();
            return WeightDivisiones;
        }

    }
}
