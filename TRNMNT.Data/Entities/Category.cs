using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Category
    {
        [Key] public Guid CategoryId { get; set; }
        public String Name { get; set; }
        public Guid EventId { get; set; }
        public int MatchTime { get; set; }
        public DateTime? CompleteTs { get; set; }

        [JsonIgnore] public virtual Event Event { get; set; }
        public virtual ICollection<WeightDivision> WeightDivisions { get; set; }
        [JsonIgnore] public virtual ICollection<Participant> Participants { get; set; }
        [JsonIgnore] public virtual ICollection<Match> Matches { get; set; }
    }
}