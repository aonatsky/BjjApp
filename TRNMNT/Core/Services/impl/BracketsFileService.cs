using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using TRNMNT.Core.Model;
using TRNMNT.Core.Service;
using Newtonsoft.Json;
using System.Linq;
using OfficeOpenXml;
using System;
using TRNMNT.Core.Const;

namespace TRNMNT.Core.Services.impl
{
    public class BracketsFileService
    {
        IFighterService fighterService;
        IHostingEnvironment env;

        public BracketsFileService(IFighterService fighterService, IHostingEnvironment env)
        {
            this.env = env;
            this.fighterService = fighterService;
        }

        public CustomFile GetBracketsFile(FighterFilterModel filter)
        {
            var models = fighterService.GetOrderedListForBrackets(filter);
            var settings = GetSettings(models.Count);
            var categoryName = filter.Categories.FirstOrDefault()?.Name;
            var weightDivisionName = filter.WeightDivisions.FirstOrDefault()?.Name;


            if (settings != null)
            {
                var filepath = FilePathService.GetBracketsFilePath(env.WebRootPath, settings.Count);

                if (!File.Exists(filepath))
                {
                    throw new Exception($"Bracket file {filepath} does not exist");
                }

                byte[] byteArray;
                var stream = new FileStream(filepath, FileMode.Open);
                using (var excelPackage = new ExcelPackage(stream))
                {
                    var sheet = excelPackage?.Workbook?.Worksheets[1];
                    if (sheet != null)
                    {
                        sheet.Cells[settings.TitleCell].Value = $"{categoryName}/{weightDivisionName}";
                        for (int i = 0; i < settings.Count; i++)
                        {
                            var fighter = models.ElementAtOrDefault(i);
                            sheet.Cells[settings.NameCells[i]].Value = fighter != null ? $"{i}. {fighter.FirstName} {fighter.LastName}" : " - ";
                        }
                    }

                    byteArray = excelPackage.GetAsByteArray();
                    excelPackage.Dispose();
                }
                stream.Dispose();

                return new CustomFile
                {
                    Name = $"Brackets_{models.Count()}_{categoryName}_{weightDivisionName}.xlsx",
                    ByteArray = byteArray,
                    ContentType = ContentTypes.EXCEL_CONTENT_TYPE
                };
                    
            }
            else
            {
                throw new Exception($"Brackets settings are not found for count {models.Count}");
            }


        }


        #region settings


        private class BracketFileSettings
        {
            public int Count { get; set; }
            public string TitleCell { get; set; }
            public string[] NameCells { get; set; }
        }

        private BracketFileSettings GetSettings(int fightersCount)
        {
            string stringJson = File.ReadAllText(Path.Combine(env.WebRootPath, "Config", "barcketsSettings.json"));
            var settingsList = JsonConvert.DeserializeObject<BracketFileSettings[]>(stringJson).ToList();
            return settingsList.FirstOrDefault(s => s.Count == fightersCount);
        }
        #endregion

    }
}
