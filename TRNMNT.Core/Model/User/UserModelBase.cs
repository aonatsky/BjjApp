using System;

namespace TRNMNT.Core.Model.User
{
    public abstract class UserModelBase
    {
        public string UserId {get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public Guid? TeamId { get; set; }
    }
}