using System;
using System.Collections.Generic;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IRoundService
    {
        IEnumerable<Round> GetRoundStructure(Participant[] participants, Guid bracketId);
    }
}