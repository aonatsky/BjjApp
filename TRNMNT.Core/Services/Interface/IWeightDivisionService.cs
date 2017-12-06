using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Services.Interface
{
    public interface IWeightDivisionService
    {
        Task<IEnumerable<WeightDivisionModelBase>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId);
    }
}
