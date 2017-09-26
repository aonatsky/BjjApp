using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Participant
    {
        [Key]
        public Guid ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime UpdateTS { get; set; }
        public string UserId{ get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

        public Guid TeamId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid WeightDivisionId { get; set; }
        public Guid EventId { get; set; }
        public String ActivatedPromoCode { get; set; }

        [JsonIgnore]
        public virtual Team Team { get; set; }
        [JsonIgnore]
        public virtual WeightDivision WeightDivision{ get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual Event Event{ get; set; }

    }
}
