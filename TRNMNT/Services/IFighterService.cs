using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Services
{
    public interface IFighterService
    {
      IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID);
    }
}
