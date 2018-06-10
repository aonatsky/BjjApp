using System;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Model.Round
{
    public class RoundResultModel
    {
        public Guid RoundId { get; set; }
        public Guid WinnerParticipantId { get; set; }
        public int CompleteTime { get; set; }
        public MatchResultTypeEnum RoundResultType { get; set; }
        public SubmissionTypeEnum SubmissionType { get; set; }
        public int AParticipantPoints { get; set; }
        public int AParticipantAdvantages { get; set; }
        public int AParticipantPenalties { get; set; }
        public int SecondParticipantPoints { get; set; }
        public int SecondParticipantAdvantages { get; set; }
        public int SecondParticipantPenalties { get; set; }
    }
}
