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
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class BracketsFileService
    {
        #region Dependencies

        private readonly IParticipantProcessingService _participantProcessingService;
        private readonly IHostingEnvironment _env;

        #endregion

        #region .ctor

        public BracketsFileService(IParticipantProcessingService participantProcessingService, IHostingEnvironment env)
        {
            _env = env;
            _participantProcessingService = participantProcessingService;
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
                                    ? $"{i + 1}. {participant.FirstName} {participant.LastName}"
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
