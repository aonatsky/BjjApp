using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IFighterService
    {
      IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID);
      bool ProcessFighterListFromFile(Stream stream);
    }
}
