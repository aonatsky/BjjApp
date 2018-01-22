using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;

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
        /// <summary>
        /// Returns bracket file
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <returns></returns>
        Task<CustomFile> GetBracketFileAsync(Guid weightDivisionId);
    }
}
