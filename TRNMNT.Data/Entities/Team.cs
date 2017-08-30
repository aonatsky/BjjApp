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
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime UpdateTs { get; set; }
        public string UpdateBy { get; set; }
        public Guid FederationId { get; set; }

        [JsonIgnore]
        public ICollection<Participant> Participants { get; set; }
        public Federation Federation { get; set; }

    }
}