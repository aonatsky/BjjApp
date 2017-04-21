using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TRNMNT.Core.Services;
using TRNMNT.Core.Data.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class FighterController : BaseController
    {
        IFighterService fighterService;
        IBracketsService bracketService;
        FighterListFileService fileService;

        public FighterController(IFighterService fighterService, IBracketsService bracketService, ILogger<FighterController> logger, FighterListFileService fileService) : base(logger)
        {
            this.fighterService = fighterService;
            this.bracketService = bracketService;
            this.fileService = fileService;
        }

        [HttpPost("[action]")]

        public async Task<String> UploadList(IFormFile file)
        {
            try
            {
                return fileService.ProcessFile(file).ToString();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return ex.Message;
            }

        }

        
        [HttpPost("[action]")]
        public async Task<IEnumerable<FighterModel>> GetFightersByFilter([FromBody] FighterFilterModel filter)
        {
            try
            {
                return fighterService.GetFighterModels.ProcessFile(file).ToString();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return ex.Message;
            }

        }


        // GET api/values
        [HttpGet]
        public IEnumerable<FighterModel> Get()
        {
            Response.StatusCode = 200;
            return fighterService.GetFighterModels();

        }


       

        [HttpGet("[action]")]
        public void Test()
        {
            bracketService.Test();
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
                }
                else
                {
                    listB.Add(fighter ?? "");
                }
            }

            var sideB = GetBracketSide(listB);
            sideB.Reverse();
            result = GetBracketSide(listA);
            result.AddRange(sideB);
            return result;
        }

        private List<Pair> GetBracketSide(List<string> list)
        {
            var result = new List<Pair>();
            var halfCount = list.Count / 2;
            for (int i = 0; i < halfCount; i++)
            {
                result.Add(new Pair(list[i], list[halfCount + i]));
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












      
    }
}
