namespace TRNMNT.Core.Const
{
    public static class EventStatus
    {
        public const string Draft = "draft";
        public const string RegistrationNotStarted = "registration_not_started";
        public const string EarlyRegistartion = "early_registration";
        public const string LateRegistration = "late_registration";
        public const string RegistrationEnded = "registration_ended";

        public static string GetTranslationKey(string status)
        {
            if(string.IsNullOrEmpty(status)){
                return "";
            }
            return $"EVENT.STATUS.{status.ToUpper()}";
        }
    }
}