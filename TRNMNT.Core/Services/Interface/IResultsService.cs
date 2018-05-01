using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.Interface
{
    public interface IResultsService
    {
        /// <summary>
        /// Returns results by selected categories;
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds);
    }
}
