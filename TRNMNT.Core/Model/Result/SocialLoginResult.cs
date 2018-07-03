namespace TRNMNT.Core.Model.Result
{
    public class SocialLoginResult
    {
        public AuthTokenResult AuthTokenResult { get; set; }
        public bool IsExistingUser { get; set; }
        public UserRegistrationModel UserData {get;set;}
    }
}