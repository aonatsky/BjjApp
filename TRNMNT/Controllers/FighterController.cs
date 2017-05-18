using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TRNMNT.Core.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Services.impl;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Controllers
{
    [Route("api/[controller]")]
    public class FighterController : CRUDController<Fighter>
    {
        IFighterService fighterService;
        FighterFileService fighterFileService;
        BracketsFileService bracketsFileService;

        public FighterController(IFighterService fighterService, ILogger<FighterController> logger, FighterFileService fighterFileService, 
            BracketsFileService bracketsFileService, IRepository<Fighter> fighterRepository) : base(logger, fighterRepository)
        {
            this.fighterService = fighterService;
            this.fighterFileService = fighterFileService;
            this.bracketsFileService = bracketsFileService;
        }

        [HttpPost("[action]")]

        public async Task<FileProcessResult> UploadList(IFormFile file)
        {
            try
            {
                return fighterFileService.ProcessFile(file);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new FileProcessResult(FileProcessResultEnum.Error);
            };
        }




        [HttpPost("[action]")]
        public async Task<IEnumerable<FighterModel>> GetFightersByFilter([FromBody] FighterFilterModel filter)
        {
            try
            {
                Response.StatusCode = 200;
                return fighterService.GetFighterModelsByFilter(filter);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                HandleException(ex);
                return new List<FighterModel>();

            }

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetBracketsFile([FromBody] FighterFilterModel filter)
        {
            try
            {
                Response.StatusCode = 200;
                var file = await bracketsFileService.GetBracketsFileAsync(filter);
                Response.Headers.Add("filename", file.Name);
                return File(file.ByteArray, file.ContentType);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                HandleException(ex);
                return null;
            }

        }















    }
}
