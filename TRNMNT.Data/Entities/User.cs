using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TRNMNT.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long FacebookId { get; set; }
        public string PictureUrl { get; set; }
        public string SocialLogin { get; set; }
        public DateTime CreateTS { get; set; }
        public DateTime UpdateTS { get; set; }
        public bool RegistrationCompleted { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<FederationMembership> FederationMemberships { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
