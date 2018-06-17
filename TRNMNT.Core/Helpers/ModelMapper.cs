using System;
using System.Collections.Generic;
using System.Text;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Helpers
{
    public static class ModelMapper
    {
        public static ParticipantSimpleModel GetParticipantSimpleModel(Participant p)
        {
            return new ParticipantSimpleModel
            {
                DateOfBirth = p.DateOfBirth,
                ParticipantId = p.ParticipantId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                TeamName = p.Team?.Name
            };
        }
    }
}
