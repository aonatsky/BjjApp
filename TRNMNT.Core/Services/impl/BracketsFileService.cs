﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Medalist;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class BracketsFileService
    {
        #region Dependencies

        private readonly IHostingEnvironment _env;

        #endregion

        #region .ctor

        public BracketsFileService(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion

        #region Public Methods

        public CustomFile GetBracketsFileAsync(List<Participant> orderedparticipants, string title)
        {
            //todo use matches
            var settings = GetSettings(orderedparticipants.Count);

            if (orderedparticipants.Count == 4 && orderedparticipants.Count(p => p != null) == 3)
            {
                settings = GetSettings(3);
            }

            if (settings != null)
            {
                var templateFilePath = Path.Combine(_env.WebRootPath, FilePath.BracketsFileFolderName, string.Concat(FilePath.BracketsFileNameMask, settings.Count, FilePath.ExcelExtension));
                if (!File.Exists(templateFilePath))
                {
                    throw new Exception($"Bracket file {templateFilePath} does not exist");
                }

                byte[] byteArray;

                using(var stream = new FileStream(templateFilePath, FileMode.Open))
                {
                    using(var excelPackage = new ExcelPackage(stream))
                    {
                        var sheet = excelPackage.Workbook?.Worksheets[1];
                        if (sheet != null)
                        {
                            sheet.Cells[settings.TitleCell].Value = title;
                            for (var i = 0; i < settings.Count; i++)
                            {
                                var participant = orderedparticipants.ElementAtOrDefault(i);
                                sheet.Cells[settings.NameCells[i]].Value = !string.IsNullOrEmpty(participant?.FirstName) ?
                                    $"{i + 1}. {participant.FirstName} {participant.LastName}" :
                                    " - ";
                            }
                        }

                        byteArray = excelPackage.GetAsByteArray();
                    }
                }

                return new CustomFile
                {
                    ByteArray = byteArray,
                        ContentType = ContentTypes.ExcelContentType
                };
            }

            throw new Exception($"Brackets settings are not found for count {orderedparticipants.Count}");
        }

        public CustomFile GetBracketsFileAsync(List<Match> firstMatches, string title)
        {
            var settings = GetSettings(firstMatches.Count * 2);

            if (firstMatches.Count == 2 && firstMatches.Any(m => m.MatchType == (int) MatchTypeEnum.Buffer))
            {
                settings = GetSettings(3);
            }

            if (settings != null)
            {
                var templateFilePath = Path.Combine(_env.WebRootPath, FilePath.BracketsFileFolderName, string.Concat(FilePath.BracketsFileNameMask, settings.Count, FilePath.ExcelExtension));
                if (!File.Exists(templateFilePath))
                {
                    throw new Exception($"Bracket file {templateFilePath} does not exist");
                }

                byte[] byteArray;

                using(var stream = new FileStream(templateFilePath, FileMode.Open))
                {
                    using(var excelPackage = new ExcelPackage(stream))
                    {
                        var sheet = excelPackage.Workbook?.Worksheets[1];
                        if (sheet != null)
                        {
                            sheet.Cells[settings.TitleCell].Value = title;
                            for (var i = 0; i < settings.Count; i = i + 2)
                            {
                                var match = firstMatches.ElementAtOrDefault(i / 2);
                                sheet.Cells[settings.NameCells[i]].Value = !string.IsNullOrEmpty(match.AParticipant?.FirstName) ?
                                    $"{i + 1}. {match.AParticipant.FirstName} {match.AParticipant.LastName}" :
                                    " - ";
                                sheet.Cells[settings.NameCells[i + 1]].Value = !string.IsNullOrEmpty(match.BParticipant?.FirstName) ?
                                    $"{i + 1}. {match.BParticipant.FirstName} {match.BParticipant.LastName}" :
                                    " - ";
                            }
                        }

                        byteArray = excelPackage.GetAsByteArray();
                    }
                }

                return new CustomFile
                {
                    ByteArray = byteArray,
                        ContentType = ContentTypes.ExcelContentType
                };
            }

            throw new Exception($"Brackets settings are not found for {firstMatches.Count} matches");
        }

        public CustomFile GetPersonalResultsFileAsync(List<CategoryWeightDivisionMedalistGroup> categoryWeightDivisionMedalistGroups)
        {
            using(var excelPackage = new ExcelPackage())
            {
                foreach (var categoryWeightDivisionMedalistGroup in categoryWeightDivisionMedalistGroups)
                {
                    var workSheet =
                        excelPackage.Workbook.Worksheets.Add(categoryWeightDivisionMedalistGroup.CategoryName);
                    var rowIndex = 1;
                    foreach (var wdGroup in categoryWeightDivisionMedalistGroup.WeightDivisionMedalistGroups)
                    {
                        workSheet.Cells[rowIndex, 1].Value = wdGroup.WeightDivisionName;
                        workSheet.Cells[rowIndex, 1].Style.Font.Bold = true;
                        for (var i = 1; i <= wdGroup.Medalists.Count; i++)
                        {
                            var medalist = wdGroup.Medalists[i - 1];
                            workSheet.Cells[rowIndex + i, 1].Value = medalist.Place;
                            workSheet.Cells[rowIndex + i, 1].Style.Font.Bold = true;
                            workSheet.Cells[rowIndex + i, 2].Value =
                                $"{medalist.Participant.FirstName} {medalist.Participant.LastName}";
                            workSheet.Cells[rowIndex + i, 3].Value =
                                medalist.Participant.TeamName;
                        }
                        rowIndex += 4;
                    }
                }
                return new CustomFile
                {
                    ByteArray = excelPackage.GetAsByteArray(),
                        ContentType = ContentTypes.ExcelContentType
                };
            }
        }

        #endregion

        #region Settings

        public class BracketFileSettings
        {
            public int Count { get; set; }
            public string TitleCell { get; set; }
            public string[] NameCells { get; set; }
        }

        private BracketFileSettings GetSettings(int fightersCount)
        {
            var stringJson = File.ReadAllText(Path.Combine(_env.WebRootPath, "Config", "barcketsSettings.json"));
            var settingsList = JsonConvert.DeserializeObject<BracketFileSettings[]>(stringJson).ToList();
            return settingsList.FirstOrDefault((BracketFileSettings s) => s.Count == fightersCount);
        }

        #endregion
    }
}