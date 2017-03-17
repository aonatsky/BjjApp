using System;
using System.IO;
using System.Linq;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Services
{
    public interface IFighterService
    {
      IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID);
      FileProcessResultEnum ProcessFighterListFromFile(Stream stream);
    }
}
