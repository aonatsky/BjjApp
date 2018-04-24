using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Round;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IRoundService
    {
        /// <summary>
        /// Creates round structure for round.
        /// </summary>
        /// <param name="participants">Array of participnats</param>
        /// <param name="bracketId">Bracket id</param>
        /// <returns></returns>
        ICollection<Round> CreateRoundStructure(Participant[] participants, Guid bracketId);
        /// <summary>
        /// Updates round
        /// </summary>
        /// <param name="round"></param>
        void UpdateRound(Round round);

        /// <summary>
        /// Sets round result
        /// </summary>
        /// <param name="model">RoundResult model</param>
        /// <returns></returns>
        Task SetRoundResultAsync(RoundResultModel model);
    }
}