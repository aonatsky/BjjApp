using System.Collections.Generic;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IWeightDivisionService
    {
        List<WeightDivision> GetWeghtClasses();
    }
}
