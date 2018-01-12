using System;

namespace TRNMNT.Core.Model.Round
{
    public class RoundModel
    {
        public Guid RoundId { get; set; }
        public string FirstParticipantName { get; set; }
        public string SecondParticipantName { get; set; }
        public Guid FirstParticipantId { get; set; }
        public Guid SecondParticipantId { get; set; }
        public Guid? NextRoundId { get; set; }
    }
}
