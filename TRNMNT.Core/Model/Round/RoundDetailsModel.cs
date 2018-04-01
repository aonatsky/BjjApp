namespace TRNMNT.Core.Model.Round
{
    public class RoundDetailsModel
    {
        public string RoundId { get; set; }
        public int FirstPlayerPenalty { get; set; }
        public int SecondPlayerPenalty { get; set; }
        public int FirstPlayerAdvantage { get; set; }
        public int SecondPlayerAdvantage { get; set; }
        public int FirstPlayerPoints { get; set; }
        public int SecondPlayerPoints { get; set; }

        public int Countdown { get; set; }
        public bool IsStarted { get; set; }
        public bool IsPaused { get; set; }
        public bool IsCompleted { get; set; }
    }
}
