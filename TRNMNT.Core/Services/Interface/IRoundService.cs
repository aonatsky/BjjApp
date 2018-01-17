using System;
using System.Collections.Generic;
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
    }
}