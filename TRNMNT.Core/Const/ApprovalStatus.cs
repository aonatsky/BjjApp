namespace TRNMNT.Core.Const
{
    public static class ApprovalStatus
    {
        public const string Approved = "approved";
        public const string Declined = "declined";
        public const string Pending = "pending";
        public const string PaymentNotFound = "paymentNotFound";

        public static string GetTranslationKey(string status)
        {
            if(string.IsNullOrEmpty(status)){
                return "";
            }
            return $"COMMON.APPROVAL.{status.ToUpper()}";
        }
    }
}