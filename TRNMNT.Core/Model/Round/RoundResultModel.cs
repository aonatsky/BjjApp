using System;
using System.Collections.Generic;
using System.Text;

namespace TRNMNT.Core.Model.Round
{
    public class RoundResultModel
    {
        public Guid RoundId { get; set; }
        public Guid WinnerParticipantId { get; set; }
        public int CompleteTime { get; set; }
        public int RundResultType { get; set; }
        public int FirstParticipantPoints { get; set; }
        public int FirstParticipantAdvantages { get; set; }
        public int FirstParticipantPenalties { get; set; }
        public int SecondParticipantPoints { get; set; }
        public int SecondParticipantAdvantages { get; set; }
        public int SecondParticipantPenalties { get; set; }
        public int SubmissionType { get; set; }
    }
}
