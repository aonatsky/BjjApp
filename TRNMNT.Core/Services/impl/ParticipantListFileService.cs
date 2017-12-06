using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantListFileService : BaseFileService
    {
        private IFighterService fighterService;
        public ParticipantListFileService(IHostingEnvironment env, IFighterService fighterService) :base(env)
        {
            this.fighterService = fighterService;
        }

        #region Public Methods
        
        

        #endregion
        

        #region Overrides
        protected override string GetFileUploadPath(string rootPath)
        {
            var directoryPath = Path.Combine(rootPath, FilePath.FIGHTERLIST_FOLDER_NAME);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return Path.Combine(directoryPath,$"{FilePath.FIGHTERLIST_FILE_NAME}_{DateTime.UtcNow.ToString("yyyy.mm.dd")}.{FilePath.EXCEL_EXTENSION}");
        }
        protected override FileProcessResult ProcessInternal(Stream stream) 
        {
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    var sheet = excelPackage?.Workbook?.Worksheets[1];
                    if (sheet != null)
                    {
                        var fighterModelList = new List<FighterModel>();
                        for (var i = 2; i <= sheet.Dimension.Rows; i++)
                        {
                            fighterModelList.Add(new FighterModel()
                            {
                                FighterId = Guid.NewGuid(),
                                FirstName = sheet.Cells[i, 1].GetValue<string>(),
                                LastName = sheet.Cells[i, 2].GetValue<string>(),
                                DateOfBirth = sheet.Cells[i, 3].GetValue<string>(),
                                Team = sheet.Cells[i, 4].GetValue<string>(),
                                WeightDivision = sheet.Cells[i, 5].GetValue<string>(),
                                Category = sheet.Cells[i, 6].GetValue<string>()
                            });
                        }
                        var message = fighterService.AddFightersByModels(fighterModelList);
                        if (String.IsNullOrEmpty(message))
                        {
                            return new FileProcessResult(FileProcessResultEnum.Success);
                        }
                        else
                        {
                            return new FileProcessResult(FileProcessResultEnum.SuccessWithErrors,message);
                        }
                    }
                    else
                    {
                        return new FileProcessResult(FileProcessResultEnum.FileIsInvalid);
                    }
                }
            }
        }

       
        #endregion

        #region Private methods

        #endregion

    }
}
