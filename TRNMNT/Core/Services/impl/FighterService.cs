using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using TRNMNT.Core.Data;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;


namespace TRNMNT.Core.Services.impl
{
    public class FighterService : IFighterService
    {
        private IAppDbContext context;
        private IRepository<Fighter> fighterRepository;
        public FighterService(IAppDbContext context, IRepository<Fighter> fighterRepository)
        {
            this.fighterRepository = fighterRepository;
            this.context = context;
        }


        public bool ProcessFighterListFromFile(Stream stream)
        {
            using (var excelPackage = new ExcelPackage(stream))
            {
                var sheet = excelPackage?.Workbook?.Worksheets[1];
                if (sheet != null)
                {
                    var fighters = new List<Fighter>();
                    for (int i = 1; i < sheet.Dimension.Rows; i++)
                    {

                        fighters.Add(new Fighter()
                        {
                            FighterID = Guid.NewGuid(),
                            FirstName = sheet.Cells[i, 1].GetValue<string>(),
                            LastName = sheet.Cells[i, 2].GetValue<string>()
                        });
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID)
        {
            return context.Fighter;
        }

    }
}
