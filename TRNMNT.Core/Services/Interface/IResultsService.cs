using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IResultsService
    {
        /// <summary>
        /// Returns results by selected categories;
        /// </summary>
        /// <param name="categoryIds">Category Id</param>
        /// <returns></returns>
        Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds);

        Task<IEnumerable<Participant>> GetMedalistsForAbsolute(Guid categoryId, bool includeAbsolute);

    }
}
