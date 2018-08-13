using System;
namespace TRNMNT.Core.Model.User
{
    public class UserModelRegistration : UserModelBase
    {
        public string Password { get; set; }
        public bool IsTeamOwner { get; set; }
    }
}