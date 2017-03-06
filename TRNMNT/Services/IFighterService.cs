using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using TRNMNT.Data.Entities;

namespace TRNMNT.Services
{
    public interface IFighterService
    {
      IQueryable<Fighter> GetFightersByWeightDivision(Guid WeightDivisionID);
      bool ProcessFighterListFromFile(Stream stream);
    }
}
