using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Participant;
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

        ///<summary>
        /// Returns brackets for category weightdivisions
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>list of weight division id and bracket model</returns>
        Task<Dictionary<string, BracketModel>> GetBracketsByCategoryAsync(Guid categoryId);
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
        /// <summary>
        /// Get winner for bracket by selected weight division
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <returns></returns>
        Task<Participant> GetWinnerAsync(Guid weightDivisionId);
        /// <summary>
        /// Get all winners for by selected weight division ids
        /// </summary>
        /// <param name="weightDivisionIds"></param>
        /// <returns></returns>
        Task<List<ParticipantSmallTableMobel>> GetWinnersAsync(IEnumerable<Guid> weightDivisionIds);
    }
}
