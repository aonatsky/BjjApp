namespace TRNMNT.Core.Model.Round
{
    public class MatchDetailsModel
    {
        public string MatchId { get; set; }
        public int AParticipantPenalties { get; set; }
        public int BParticipantPenalties { get; set; }
        public int AParticipantAdvantages { get; set; }
        public int BParticipantAdvantages { get; set; }
        public int AParticipantPoints { get; set; }
        public int BParticipantPoints { get; set; }

        public int Countdown { get; set; }
        public bool IsStarted { get; set; }
        public bool IsPaused { get; set; }
        public bool IsCompleted { get; set; }
    }
}
