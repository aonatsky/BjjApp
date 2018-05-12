using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Services.Interface
{
    public interface IBracketService
    {
        /// <summary>
        /// Returns results by selected categories;
        /// </summary>
        /// <param name="categoryIds">Category Id</param>
        /// <returns></returns>
        Task<IEnumerable<TeamResultModel>> GetTeamResultsByCategoriesAsync(IEnumerable<Guid> categoryIds);
        
        ///<summary>
        /// Returns bracket for weightdivision
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <returns>Bracket model</returns>
        Task<BracketModel> GetBracketModelAsync(Guid weightDivisionId);
        
        /// <summary>
        /// Returns bracket for weightdevision. Marked as started. 
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <returns>Bracket model</returns>
        Task<BracketModel> RunBracketAsync(Guid weightDivisionId);

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
        /// Get all winners for selected categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<ParticipantInAbsoluteDivisionModel>> GetWinnersAsync(Guid categoryId);

        Task<bool> IsCategoryCompletedAsync(Guid categoryId);

        Task ManageAbsoluteWeightDivisionAsync(CreateAbsoluteDivisionModel model);

        /// <summary>
        /// Sets round result
        /// </summary>
        /// <param name="model">RoundResult model</param>
        /// <returns></returns>
        Task SetRoundResultAsync(RoundResultModel model);

        /// <summary>
        /// Returns file with personal results;
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        Task<CustomFile> GetPersonalResultsFileByCategoriesAsync(
            IEnumerable<Guid> categoryIds);
    }
}
