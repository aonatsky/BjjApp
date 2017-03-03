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
        private readonly IWeightClassService _weightClassService;
        public EventDetailsController(IWeightClassService weightClassService)
        {
            _weightClassService = weightClassService;
        }
        // GET api/values
        [HttpGet("[action]")]
        public IEnumerable<WeightClass> GetWeightClasses()
        {
            var weightClasses = _weightClassService.GetWeghtClasses();
            return weightClasses;
        }

    }
}
