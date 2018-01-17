using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IBracketService
    {
        ///<summary>
        /// Returns bracket for weightdivision
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <returns>Bracket model</returns>
        Task<BracketModel> GetBracketAsync(Guid weightDivisionId);
        /// <summary>
        /// Updates bracket from model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateBracket(BracketModel model);
    }
}
