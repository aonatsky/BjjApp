using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{
    public class Federation
    {
        [Key]
        public Guid FederationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdateTs { get; set; }
        public string OwnerId { get; set; }
        public int MembershipPrice { get; set; }
        public int TeamRegistrationPrice { get; set; }
        public String Currency { get; set; }
        public string ContactInformation { get; set; }
        public string ImgPath { get; set; }


        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        public virtual ICollection<FederationMembership> FederationMemberships { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
