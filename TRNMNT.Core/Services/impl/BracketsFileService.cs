using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using TRNMNT.Core.Const;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Services.Impl
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

        public async Task<CustomFile> GetBracketsFileAsync(FighterFilterModel filter)
        {
            var models = fighterService.GetOrderedListForBrackets(filter);
            var settings = GetSettings(models.Count);


            if (settings != null)
            {
                var templateFilepath = Path.Combine(env.WebRootPath, FilePath.BRACKETS_FILE_FOLDER_NAME, FilePath.BRACKETS_FILE_NAME_MASK + settings.Count.ToString() + FilePath.EXCEL_EXTENSION);
                if (!File.Exists(templateFilepath))
                {
                    throw new Exception($"Bracket file {templateFilepath} does not exist");
                }

                byte[] byteArray;
                var stream = new FileStream(templateFilepath, FileMode.Open);
                using (var excelPackage = new ExcelPackage(stream))
                {
                    var sheet = excelPackage?.Workbook?.Worksheets[1];
                    if (sheet != null)
                    {
                        for (var i = 0; i < settings.Count; i++)
                        {
                            var fighter = models.ElementAtOrDefault(i);
                            sheet.Cells[settings.NameCells[i]].Value = !string.IsNullOrEmpty(fighter.FirstName) ? $"{i+1}. {fighter.FirstName} {fighter.LastName}" : " - ";
                        }
                    }

                    byteArray = excelPackage.GetAsByteArray();
                    excelPackage.Dispose();
                }
                stream.Dispose();

                return new CustomFile
                {
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
            var stringJson = File.ReadAllText(Path.Combine(env.WebRootPath, "Config", "barcketsSettings.json"));
            var settingsList = JsonConvert.DeserializeObject<BracketFileSettings[]>(stringJson).ToList();
            return settingsList.FirstOrDefault(s => s.Count == fightersCount);
        }
        #endregion

    }
}
