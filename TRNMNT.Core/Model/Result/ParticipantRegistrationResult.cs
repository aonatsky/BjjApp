namespace TRNMNT.Core.Model.Result
{
    public class ParticipantRegistrationResult
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
        public string ParticipantId { get; set; }
        public PaymentDataModel PaymentData { get; set; }

        public ParticipantRegistrationResult() { }

        public ParticipantRegistrationResult(bool success, string participantId, PaymentDataModel paymentdataModel = null, string reason = "")
        {
            Success = success;
            Reason = reason;
        }
    }
}
