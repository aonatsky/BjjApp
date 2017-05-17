using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Service;

namespace TRNMNT.Core.Services.impl
{
    public class FighterFileService : BaseFileService
    {
        private IFighterService fighterService;
        public FighterFileService(IHostingEnvironment env, IFighterService fighterService) :base(env)
        {
            this.fighterService = fighterService;
        }

        #region Public Methods
        
        

        #endregion
        

        #region Overrides
        protected override string GetFileUploadPath(string rootPath)
        {
            var directoryPath = FilePathService.GetFighterListUploadFolder(rootPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return FilePathService.GetFighterListFilePath(rootPath);
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
                        for (int i = 2; i <= sheet.Dimension.Rows; i++)
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
