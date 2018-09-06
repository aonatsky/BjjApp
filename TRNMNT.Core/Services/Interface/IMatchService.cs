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
        /// Returns matches for corresponding category/weightdivision asynchronous
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<Match>> GetMatchesAsync(Guid categoryId, Guid weightDivisionId);

        /// <summary>
        /// Returns first matches for corresponding category/weightdivision asynchronous
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<Match>> GetFirstMatchesAsync(Guid categoryId, Guid weightDivisionId);

        /// <summary>
        /// Updates matches from models asynchronous
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

        /// <summary>
        /// Sets match result asynchronous
        /// </summary>
        /// <param name="model">Result model</param>
        /// <returns></returns>
        Task SetMatchResultAsync(MatchResultModel model);

        /// <summary>
        /// Checks if matches created for event asynchronous
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task<bool> AreMatchesCreatedForEventAsync(Guid eventId);

        /// <summary>
        /// Deletes matches for event async
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        Task DeleteMatchesForEventAsync(Guid eventId);

        /// <summary>
        /// Creates matches for category and weightdivision
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="weightDivisionId"></param>
        /// <returns></returns>
        List<Match> CreateMatchesAsync(Guid categoryId, Guid weightDivisionId, List<Participant> orderedParticipants);
    }
}