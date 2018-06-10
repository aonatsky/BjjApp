using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Round;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IMatchService
    {
        /// <summary>
        /// Returns matches for corresponding category/weightdivision
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<Match>> GetMatchesAsync(Guid categoryId, Guid weightDivisionId);

        /// <summary>
        /// Updates matches from models
        /// </summary>
        /// <param name="matchModels"></param>
        /// <returns></returns>
        Task UpdateMatchesParticipantsAsync(List<MatchModel> matchModels);

        /// <summary>
        /// Marked matches with no participant as NO CONTEST
        /// </summary>
        /// <param name="weightDivision"></param>
        /// <returns></returns>
        Task<List<Match>> GetProcessedMatchesAsync(Guid categoryId, Guid weightDivision);
    }
}
