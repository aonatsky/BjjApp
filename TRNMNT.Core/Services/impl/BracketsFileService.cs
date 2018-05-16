using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using TRNMNT.Core.Const;
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

        public async Task<CustomFile> GetBracketsFileAsync(List<Participant> participants)
        {
            var settings = GetSettings(participants.Count);

            if (settings != null)
            {
                var templateFilePath = Path.Combine(_env.WebRootPath, FilePath.BracketsFileFolderName, string.Concat(FilePath.BracketsFileNameMask, settings.Count, FilePath.ExcelExtension));
                if (!File.Exists(templateFilePath))
                {
                    throw new Exception($"Bracket file {templateFilePath} does not exist");
                }

                byte[] byteArray;
                using (var stream = new FileStream(templateFilePath, FileMode.Open))
                {
                    using (var excelPackage = new ExcelPackage(stream))
                    {
                        var sheet = excelPackage.Workbook?.Worksheets[1];
                        if (sheet != null)
                        {
                            for (var i = 0; i < settings.Count; i++)
                            {
                                var participant = participants.ElementAtOrDefault(i);
                                sheet.Cells[settings.NameCells[i]].Value = !string.IsNullOrEmpty(participant?.FirstName)
                                    ? $"{i + 1}. {participant.FirstName} {participant.LastName} ({participant.Team.Name})"
                                    : " - ";
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

            throw new Exception($"Brackets settings are not found for count {participants.Count}");
        }


        public CustomFile GetPersonalResultsFileAsync(List<CategoryWeightDivisionMedalistGroup> categoryWeightDivisionMedalistGroups)
        {
            using (var excelPackage = new ExcelPackage())
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
                                medalist.Participant.Team.Name;
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

        private class BracketFileSettings
        {
            public int Count { get; set; }
            public string TitleCell { get; set; }
            public string[] NameCells { get; set; }
        }

        private BracketFileSettings GetSettings(int fightersCount)
        {
            var stringJson = File.ReadAllText(Path.Combine(_env.WebRootPath, "Config", "barcketsSettings.json"));
            var settingsList = JsonConvert.DeserializeObject<BracketFileSettings[]>(stringJson).ToList();
            return settingsList.FirstOrDefault(s => s.Count == fightersCount);
        }

        #endregion
    }
}

