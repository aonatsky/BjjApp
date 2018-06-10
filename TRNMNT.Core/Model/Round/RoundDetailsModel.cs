namespace TRNMNT.Core.Model.Round
{
    public class RoundDetailsModel
    {
        public string RoundId { get; set; }
        public int AParticipantPenalties { get; set; }
        public int SecondParticipantPenalties { get; set; }
        public int AParticipantAdvantages { get; set; }
        public int SecondParticipantAdvantages { get; set; }
        public int AParticipantPoints { get; set; }
        public int SecondParticipantPoints { get; set; }

        public int Countdown { get; set; }
        public bool IsStarted { get; set; }
        public bool IsPaused { get; set; }
        public bool IsCompleted { get; set; }
    }
}
