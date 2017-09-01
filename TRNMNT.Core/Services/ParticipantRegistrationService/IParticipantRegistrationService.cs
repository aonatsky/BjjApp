﻿using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Core.Services
{
    public interface IParticipantRegistrationService
    {
         Task<ParticipantRegistrationResult> ProcessParticipantRegistrationAsync(ParticipantRegistrationModel model, string userId, string callbackUrl);
    }


}
