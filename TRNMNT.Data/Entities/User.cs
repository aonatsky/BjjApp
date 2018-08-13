using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long FacebookId { get; set; }
        public string PictureUrl { get; set; }
        public string SocialLogin { get; set; }
        public Guid? TeamId { get; set; }
        public string TeamMembershipApprovalStatus { get; set; }
        public DateTime CreateTS { get; set; }
        public DateTime? UpdateTS { get; set; }
        public bool RegistrationCompleted { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<FederationMembership> FederationMemberships { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }

        [JsonIgnore]
        public virtual Team Team { get; set; }
    }
}