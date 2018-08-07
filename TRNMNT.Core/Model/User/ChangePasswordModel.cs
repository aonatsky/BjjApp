namespace TRNMNT.Core.Model.User
{
    public class ChangePasswordModel
    {
        public string OldPassword {get;set;}
        public string NewPassword {get;set;}
        public string UserId {get;set;}
    }
}