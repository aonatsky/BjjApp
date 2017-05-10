using System;
using System.IO;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;
using TRNMNT.Core.Model;
using TRNMNT.Core.Service;

namespace TRNMNT.Core.Services
{
    public class FighterListFileService : FileUploadService
    {
        IRepository<Team> teamRepository;
        IRepository<Fighter> fighterRepository;
        IRepository<WeightDivision> weightDivisionRepository;
        IRepository<Category> categoryRepository;

        ExcelWorksheet sheet;

        public FighterListFileService(IRepository<Fighter> fighterRepository, IRepository<Team> teamRepository,
        IRepository<Category> categoryRepository, IRepository<WeightDivision> weightDivisionRepository,
        IHostingEnvironment env) : base(env)
        {
            this.teamRepository = teamRepository;
            this.fighterRepository = fighterRepository;
            this.categoryRepository = categoryRepository;
            this.weightDivisionRepository = weightDivisionRepository;
        }

        private const string DATE_FORMAT = "dd-mm-yy";

        protected override string GetFileUploadPath(string rootPath)
        {
            var directoryPath = FilePathService.GetFighterListUploadFolder(rootPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return FilePathService.GetFighterListFilePath(rootPath); 
        }

        protected override FileProcessResult PostUploadProcess(Stream stream)
        {
            try
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    sheet = excelPackage?.Workbook?.Worksheets[1];
                    if (sheet != null)
                    {
                        var result = new FileProcessResult(FileProcessResultEnum.Success);

                        var existingTeams = this.teamRepository.GetAll().ToList();
                        var existingFighters = this.fighterRepository.GetAll().ToList();

                        var fightersToAdd = new List<Fighter>();
                        var teamsToAdd = new List<Team>();
                        var comparer = new FighterComparer();

                        for (int i = 2; i <= sheet.Dimension.Rows; i++)
                        {
                            var fighter = new Fighter()
                            {
                                FighterId = Guid.NewGuid(),
                                FirstName = GetFirstName(i),
                                LastName = GetLastName(i)
                            };

                            var dob = GetDateOfBirth(i);
                            if (dob != null)
                            {
                                fighter.DateOfBirth = dob.Value;
                            }
                            else
                            {
                                result.Code = FileProcessResultEnum.SuccessWithErrors;
                                result.Message += $"Date of birth for {fighter.FirstName} {fighter.LastName} is invalid";
                                continue;
                            }

                            //Comparison by full name and dob
                            if (existingFighters.Any(f => comparer.Equals(f, fighter)))
                            {
                                continue;
                            }



                            var category = GetCategory(GetCategoryName(i));
                            if (category != null)
                            {
                                fighter.CategoryId = category.CategoryId;
                            }
                            else
                            {
                                result.Code = FileProcessResultEnum.SuccessWithErrors;
                                result.Message += $"Category {GetCategoryName(i)} is invalid ";
                                continue;
                            }


                            var weightDivision = GetWeightDivision(GetWeightDivisionName(i));
                            if (weightDivision != null)
                            {
                                fighter.WeightDivisionId = weightDivision.WeightDivisionId;
                            }
                            else
                            {
                                result.Code = FileProcessResultEnum.SuccessWithErrors;
                                result.Message += $"Weight division {GetWeightDivisionName(i)} is invalid";
                                continue;
                            }

                            var team = GetTeam(i, existingTeams, ref teamsToAdd);
                            fighter.TeamId = team.TeamId;

                            fightersToAdd.Add(fighter);
                        }

                        teamRepository.AddRange(teamsToAdd);
                        fighterRepository.AddRange(fightersToAdd);
                        fighterRepository.Save();
                        return result;
                    }
                    else
                    {
                        return new FileProcessResult(FileProcessResultEnum.FileIsEmpty);
                    }


                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #region private methods
        private string GetFirstName(int rowNumber)
        {
            return sheet.Cells[rowNumber, 1].GetValue<string>();
        }
        private string GetLastName(int rowNumber)
        {
            return sheet.Cells[rowNumber, 2].GetValue<string>();
        }

        private string GetWeightDivisionName(int rowNumber)
        {
            return sheet.Cells[rowNumber, 5].GetValue<string>();
        }

        private string GetCategoryName(int rowNumber)
        {
            return sheet.Cells[rowNumber, 6].GetValue<string>();
        }

        private DateTime? GetDateOfBirth(int rowNumber)
        {
            //DateTime dob;
            if (DateTime.TryParse(sheet.Cells[rowNumber, 3].GetValue<string>(), out var dob))
            {
                return dob;
            }
            else
            {
                return null;
            }
        }

        private Team GetTeam(int rowNumber, List<Team> teams, ref List<Team> teamsToAdd)
        {
            var teamName = sheet.Cells[rowNumber, 4].GetValue<string>();
            Team team = teamsToAdd.FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
            if (team == null)
            {
                team = teams.FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
                if (team == null)
                {
                    team = new Team
                    {
                        TeamId = Guid.NewGuid(),
                        Name = teamName
                    };
                    teamsToAdd.Add(team);
                }
            }
            return team;

        }
        private WeightDivision GetWeightDivision(string name)
        {
            return weightDivisionRepository.GetAll().FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        private Category GetCategory(string name)
        {
            return (categoryRepository.GetAll().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

        }

        private class FighterComparer : IEqualityComparer<Fighter>
        {
            public bool Equals(Fighter x, Fighter y)
            {
                return (x.FirstName == y.FirstName && x.LastName == y.LastName && x.DateOfBirth == y.DateOfBirth);
            }

            public int GetHashCode(Fighter fighter)
            {
                return fighter.FirstName.GetHashCode() ^ fighter.LastName.GetHashCode() ^ fighter.DateOfBirth.GetHashCode();
            }


        }
        #endregion
    }
}
