using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Participant
    {
        [Key]
        public Guid ParticipantId { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? UpdateTS { get; set; }
        public string UserId { get; set; }
        public bool IsDisqualified { get; set; }
        public bool IsActive { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsMember { get; set; }
        public Guid? OrderId { get; set; }

        public Guid TeamId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid EventId { get; set; }
        public Guid WeightDivisionId { get; set; }
        public Guid? AbsoluteWeightDivisionId { get; set; }
        public String ActivatedPromoCode { get; set; }

        [JsonIgnore]
        public virtual Team Team { get; set; }

        [JsonIgnore]
        public virtual WeightDivision AbsoluteWeightDivision { get; set; }

        [JsonIgnore]
        public virtual WeightDivision WeightDivision { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual Event Event { get; set; }

    }
}
