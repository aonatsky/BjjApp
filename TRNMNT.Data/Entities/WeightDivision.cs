using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{

    public class WeightDivision
    {
        [Key]
        public Guid WeightDivisionId { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Descritpion { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsAbsolute { get; set; }
        public DateTime? StartTs { get; set; }
        public DateTime? CompleteTs { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<Participant> Participants { get; set; }

        [JsonIgnore]
        public virtual ICollection<Participant> AbsoluteDivisionParticipants { get; set; }
    }
}