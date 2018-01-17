using System;
using TRNMNT.Core.Model.Participant;

namespace TRNMNT.Core.Model.Round
{
    public class RoundModel
    {
        public Guid RoundId { get; set; }
        public ParticipantModelBase FirstParticipant { get; set; }
        public ParticipantModelBase SecondParticipant { get; set; }
        public Guid? NextRoundId { get; set; }
        public int Stage { get; set; }
    }
}
