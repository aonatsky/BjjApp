using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TRNMNT.Services;
using TRNMNT.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class FighterController : Controller
    {
        IFighterService _fighterService;
        IBracketsService _bracketService;

        public FighterController(IFighterService fighterService, IBracketsService bracketService)
        {
            _fighterService = fighterService;
            _bracketService = bracketService;
            var test = fighterService.GetFightersByWeightClass(Guid.NewGuid());
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Fighter> Get()
        {
            return _fighterService.GetFightersByWeightClass(Guid.NewGuid());

        }


        [HttpGet("[action]")]
        public IEnumerable<WeightClass> GetWeightClassses()
        {
            return new List<WeightClass>(){
                new WeightClass(){WeightClassID = Guid.NewGuid(), Name = "Feather", Weight = 60},
                new WeightClass(){WeightClassID = Guid.NewGuid(), Name = "Light", Weight = 70},
                new WeightClass(){WeightClassID = Guid.NewGuid(), Name = "Medium", Weight = 80},
                new WeightClass(){WeightClassID = Guid.NewGuid(), Name = "Heavy", Weight = 90}
            };
        }

        [HttpGet("[action]")]
        public void Test()
        {
            _bracketService.Test();
        }

        [HttpGet("[action]")]
        public string GetOrdered()
        {
           //var result = GetOrdered(GetTestTeams()).AsEnumerable();
           return "result";

        }

        public class TestClass
        {
            public int Id { get; set; }
            public string Team { get; set; }

            public TestClass(int id, string team)
            {
                Id = id;
                Team = team;
            }
        }

        public class Pair
        {
            string T1 { get; set; }
            string T2 { get; set; }
            public Pair(string t1, string t2)
            {
                T1 = t1;
                T2 = t2;
            }
        }

        private List<TestClass> GetOrdered(List<TestClass> list)
        {
            var result = new List<TestClass>();
            var orderedByTeam = list.OrderBy(o => o.Team);
            var teams = list.Select(t => t.Team).Distinct();
            var count = list.Count;

            for (int i = 0; i < count; i++)
            {
                foreach (var team in teams)
                {
                    var item = list.FirstOrDefault(t => t.Team == team && !result.Contains(t));
                    if (item != null)
                    {
                        result.Add(item);
                        //list.Remove(item);
                    }
                }
            }
            return result;
        }

        private IEnumerable<Pair> GetOrdered(string[] teams)
        {
            var result = new List<Pair>();
            var orderedByCount = teams.ToList().GroupBy(t => t).OrderByDescending(g => g.Count())
            .SelectMany(t => t).ToList();
            var bracketsSzie = 16;

            var listA = new List<string>();
            var listB = new List<string>();

            for (int i = 0; i < bracketsSzie; i++)
            {
                var fighter = orderedByCount.ElementAtOrDefault(i);
                if (i % 2 == 0)
                {
                    listA.Add(fighter ?? "");
                }else{
                    listB.Add(fighter ?? "");
                }
            }
            
            var sideB = GetBracketSide(listB);
            sideB.Reverse();         
            result = GetBracketSide(listA);
            result.AddRange(sideB); 
            return result;
        }

        private List<Pair> GetBracketSide(List<string> list){
            var result = new List<Pair>();
            var halfCount = list.Count/2;
            for (int i = 0; i < halfCount; i++)
            {
                result.Add(new Pair(list[i],list[halfCount +i]));
            }
            return result;
        }





        private string[] GetTestTeams()
        {
            var teams = new string[] { "Gracie Barra", "Alliance", "ZR", "Zenith" };
            return new string[] {teams[0],teams[0],teams[0],teams[1],teams[1],teams[1],
                teams[2],teams[2],teams[0],teams[3], teams[0],teams[0]};

        }

        private List<TestClass> GetTestList()
        {
            var list = new List<TestClass>();
            var teams = GetTestTeams();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TestClass(i, teams[i]));
            }
            return list;
        }












        #region Common

        // GET api/values/5

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
