namespace TRNMNT.Core.Const
{
    public static class ApprovalStatus
    {
        public const string Approved = "approved";
        public const string Declined = "declined";
        public const string Pending = "pending";

        public static string GetTranslationKey(string name)
        {
            return $"COMMON.APPROVAL.{name.ToUpper()}";
        }
    }
}