using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TeamId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string ContactName { get; set; }

        [MaxLength(1000)]
        public string ContactEmail { get; set; }

        [MaxLength(1000)]
        public string ContactPhone { get; set; }

        public DateTime CreateTs { get; set; }
        public DateTime? UpdateTs { get; set; }
        public string CreateBy { get; set; }
        public Guid FederationId { get; set; }
        public Guid? OrderId { get; set; }
        public string ApprovalStatus { get; set; }

        [JsonIgnore]
        public ICollection<Participant> Participants { get; set; }
        public Federation Federation { get; set; }

    }
}