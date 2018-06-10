using System;
using TRNMNT.Core.Model.Participant;

namespace TRNMNT.Core.Model.Round
{
    public class MatchModel
    {
        public Guid MatchId { get; set; }
        public Guid WeightDivisionId { get; set; }
        public Guid CategoryId { get; set; }
        public ParticipantSimpleModel AParticipant { get; set; }
        public ParticipantSimpleModel BParticipant { get; set; }
        public Guid? NextMatchId { get; set; }
        public ParticipantSimpleModel WinnerParticipant { get; set; }
        public int Round { get; set; }
        public int MatchType { get; set; }
        public int MatchTime { get; set; }
        public int Order { get; set; }
        public string AParticipantResult { get; set; }
        public string BParticipantResult { get; set; }
    }

}
