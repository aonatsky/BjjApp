using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data;

namespace TRNMNT.Services
{
    public class WeightClassService : IWeightClassService
    {
        IAppDbContext _context;

        public WeightClassService(IAppDbContext context)
        {
            _context = context;
        }

        public List<WeightClass> GetWeghtClasses()
        {
            return _context.WeightClass.ToList();
        }
    }
}
