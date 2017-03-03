using System;
using System.Linq;
using TRNMNT.Data;
using TRNMNT.Data.Entities;

namespace TRNMNT.Services.impl
{
    public class FighterService: IFighterService
    {
        private IAppDbContext _context;
        public FighterService(IAppDbContext context) 
        {
            _context = context;
        }

        public IQueryable<Fighter> GetFightersByWeightClass(Guid weightClassID)
        {
            return _context.Fighter;
        }

    }
}
