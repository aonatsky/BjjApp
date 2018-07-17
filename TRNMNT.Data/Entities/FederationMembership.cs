using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{
    public class FederationMembership
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FederationMembershipId { get; set; }
        public Guid FederationId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTs { get; set; }
        public DateTime? UpdateTs { get; set; }
        public bool IsApproved { get; set; }

        public virtual Federation Federation { get; set; }
        public virtual User User { get; set; }
    }
}