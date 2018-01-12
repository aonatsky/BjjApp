using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.FileProcessingOptions;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantListFileService : BaseFileProcessiongService<ParticipantListProcessingOptions>
    {
        #region Dependencies

        private readonly IParticipantProcessingService _participantProcessingService;

        #endregion

        #region .ctor

        public ParticipantListFileService(IHostingEnvironment env, IParticipantProcessingService participantProcessingService) : base(env)
        {
            _participantProcessingService = participantProcessingService;
        }

        #endregion

        #region Overrides

        protected override string GetFileUploadPath(string rootPath)
        {
            var directoryPath = Path.Combine(rootPath, FilePath.FighterlistFolderName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return Path.Combine(directoryPath, $"{FilePath.FighterlistFileName}_{DateTime.UtcNow:yyyy.mm.dd}.{FilePath.ExcelExtension}");
        }

        protected override async Task<FileProcessResult> ProcessInternalAsync(Stream stream, ParticipantListProcessingOptions options)
        {
            using (var excelPackage = new ExcelPackage(stream))
            {
                var sheet = excelPackage.Workbook?.Worksheets[1];
                if (sheet != null)
                {
                    var fighterModelList = new List<ParticitantModel>(sheet.Dimension.Rows);
                    for (var i = 2; i <= sheet.Dimension.Rows; i++)
                    {
                        fighterModelList.Add(new ParticitantModel
                        {
                            ParticipantId = Guid.NewGuid(),
                            FirstName = sheet.Cells[i, 1].GetValue<string>(),
                            LastName = sheet.Cells[i, 2].GetValue<string>(),
                            DateOfBirth = sheet.Cells[i, 3].GetValue<string>(),
                            Team = sheet.Cells[i, 4].GetValue<string>(),
                            WeightDivision = sheet.Cells[i, 5].GetValue<string>(),
                            Category = sheet.Cells[i, 6].GetValue<string>()
                        });
                    }
                    var message = await _participantProcessingService.AddParticipantsByModelsAsync(fighterModelList, options.EventId, options.FederationId);
                    if (string.IsNullOrEmpty(message))
                    {
                        return new FileProcessResult(FileProcessResultEnum.Success);
                    }
                    return new FileProcessResult(FileProcessResultEnum.SuccessWithErrors, message);
                }
                return new FileProcessResult(FileProcessResultEnum.FileIsInvalid);
            }
        }

        #endregion
    }
}
