using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.Interface
{
    public interface IResultsService
    {
        Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds);
    }
}
