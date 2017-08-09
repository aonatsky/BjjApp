namespace TRNMNT.Core.Model.Result
{
    public class UserRegistrationResult
    {
        public bool Success { get; set; }
        public string Reason { get; set; }

        public UserRegistrationResult(bool success, string reason = "")
        {
            Success = success;
            Reason = reason;
        }
    }
}
