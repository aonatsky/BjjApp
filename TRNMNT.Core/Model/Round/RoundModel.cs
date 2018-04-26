using System;
using TRNMNT.Core.Model.Participant;

namespace TRNMNT.Core.Model.Round
{
    public class RoundModel
    {
        public Guid RoundId { get; set; }
        public ParticipantSimpleModel FirstParticipant { get; set; }
        public ParticipantSimpleModel SecondParticipant { get; set; }
        public Guid? NextRoundId { get; set; }
        public int Stage { get; set; }
        public int RoundType { get; set; }
        public int RoundTime { get; set; }
        public string FirstParticipantResult { get; set; }
        public string SecondParticipantResult { get; set; }
    }

}
