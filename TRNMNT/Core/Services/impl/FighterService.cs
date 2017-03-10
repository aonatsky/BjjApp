using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using TRNMNT.Core.Data;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;
using TRNMNT.Core.Enum;


namespace TRNMNT.Core.Services.impl
{
    public class FighterService : IFighterService
    {
        private IAppDbContext context;
        private IRepository<Fighter> fighterRepository;
        private IRepository<Team> teamRepository;
        private FileService fileService;
        public FighterService(IAppDbContext context, IRepository<Fighter> fighterRepository, FileService fileService)
        {
            this.fighterRepository = fighterRepository;
            this.context = context;
            this.fileService = fileService;
        }


        public FileProcessResultEnum ProcessFighterListFromFile(Stream stream)
        {
            try
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    var sheet = excelPackage?.Workbook?.Worksheets[1];
                    if (sheet != null)
                    {
                        var fighters = new List<Fighter>();
                        var teams = this.teamRepository.GetAll().ToList();
                        var teamsToAdd = new List<Team>();
                        
                        for (int i = 1; i < sheet.Dimension.Rows; i++)
                        {

                            var team = teams.FirstOrDefault(t => t.Name.Equals(sheet.Cells[i, 2].GetValue<string>()));
                            if (team =)
                            {
                                
                            }
                            fighters.Add(new Fighter()
                            {
                                FighterID = Guid.NewGuid(),
                                FirstName = sheet.Cells[i, 1].GetValue<string>(),
                                LastName = sheet.Cells[i, 2].GetValue<string>()

                            });
                        }
                        return FileProcessResultEnum.Success;
                    }
                    else
                    {
                        return FileProcessResultEnum.FileIsEmpty;
                    }
                }
            }
            catch (System.Exception ex)
            {
                return FileProcessResultEnum.FileIsInvalid;
            }

        }


        public IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID)
        {
            return context.Fighter;
        }

    }
}
