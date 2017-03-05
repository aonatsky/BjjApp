using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using TRNMNT.Data;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;


namespace TRNMNT.Services.impl
{
    public class FighterService: IFighterService
    {
        private IAppDbContext context;
        private IRepository<Fighter> fighterRepository;
        public FighterService(IAppDbContext context, IRepository<Fighter> fighterRepository) 
        {
            this.fighterRepository = fighterRepository;
            this.context = context;
        }


        public void ProcessFighterListFromStream(Stream stream){
            var package = new ExcelPackage(stream);
        }

        public IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID)
        {
            return context.Fighter;
        }

    }
}
