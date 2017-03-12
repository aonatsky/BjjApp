using System;
using System.IO;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;

namespace TRNMNT.Core.Services
{
    public class FighterListFileService : FileService
    {
        IRepository<Team> teamRepository;
        IRepository<Fighter> fighterRepository;

        public FighterListFileService(IRepository<Fighter> fighterRepository, IRepository<Team> teamRepository,
        IHostingEnvironment env) : base(env)
        {
            this.teamRepository = teamRepository;
            this.fighterRepository = fighterRepository;
        }

        private const string FIGHTERLIST_FOLDER = "\\FighterList";
        private const string FIGHTERLIST_FILE = "\\List";


        protected override string GetFilePath(string rootPath)
        {
            return Path.Combine(rootPath, FIGHTERLIST_FOLDER, $"{FIGHTERLIST_FILE}_{DateTime.UtcNow.ToString("yyyy.mm.dd")}");
        }


        protected override FileProcessResultEnum PostUploadProcess(Stream stream)
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
                            var team = teams.FirstOrDefault(t => t.Name.Equals(GetTeamName(sheet,i)));
                            if (team == null)
                            {
                                team = new Team()
                                {
                                    TeamID = Guid.NewGuid(),
                                    Name = GetTeamName(sheet,i)
                                };
                                teamsToAdd.Add(team);
                            }
                            fighters.Add(new Fighter()
                            {
                                FighterID = Guid.NewGuid(),
                                FirstName = GetFirstName(sheet,i),
                                LastName = GetLastName(sheet,i),
                                TeamID = team.TeamID
                            });

                            teamRepository.AddRange(teamsToAdd);
                            fighterRepository.AddRange(fighters);
                            fighterRepository.Save();
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

        private string GetFirstName(ExcelWorksheet sheet, int rowNumber)
        {
            return sheet.Cells[rowNumber, 1].GetValue<string>();
        }
        private string GetLastName(ExcelWorksheet sheet, int rowNumber)
        {
            return sheet.Cells[rowNumber, 2].GetValue<string>();
        }
        private string GetTeamName(ExcelWorksheet sheet, int rowNumber)
        {
            return sheet.Cells[rowNumber, 3].GetValue<string>();
        }
    }
}
