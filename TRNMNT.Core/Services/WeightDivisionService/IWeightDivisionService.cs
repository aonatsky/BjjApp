using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IWeightDivisionService
    {
        Task<List<WeightDivision>> GetWeightDivisionsByCategoryId(Guid categoryId);
    }
}
