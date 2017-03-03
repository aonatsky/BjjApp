using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data;

namespace TRNMNT.Services
{
    public class WeightDivisionService : IWeightDivisionService
    {
        IAppDbContext _context;

        public WeightDivisionService(IAppDbContext context)
        {
            _context = context;
        }

        public List<WeightDivision> GetWeghtClasses()
        {
            return _context.WeightDivision.ToList();
        }
    }
}
