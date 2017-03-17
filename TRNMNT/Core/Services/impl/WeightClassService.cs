using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data;

namespace TRNMNT.Core.Services
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
