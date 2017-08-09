namespace TRNMNT.Core.Model.Result
{
    public class ParticipantRegistrationResult
    {
        public bool Success { get; set; }
        public string Reason { get; set; }

        public ParticipantRegistrationResult(bool success, string reason = "")
        {
            Success = success;
            Reason = reason;
        }
    }
}
