using System;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Model.Round
{
    public class MatchResultModel
    {
        public Guid MatchId { get; set; }
        public Guid WinnerParticipantId { get; set; }
        public int CompleteTime { get; set; }
        public MatchResultTypeEnum RoundResultType { get; set; }
        public SubmissionTypeEnum SubmissionType { get; set; }
        public int AParticipantPoints { get; set; }
        public int AParticipantAdvantages { get; set; }
        public int AParticipantPenalties { get; set; }
        public int BParticipantPoints { get; set; }
        public int BParticipantAdvantages { get; set; }
        public int BParticipantPenalties { get; set; }
    }
}
