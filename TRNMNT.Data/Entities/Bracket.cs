using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Bracket
    {
        [Key]
        public Guid BracketId { get; set; }
        public Guid WeightDivisionId { get; set; }

        [JsonIgnore]
        public virtual WeightDivision WeightDivision { get; set; }
        [JsonIgnore]
        public virtual ICollection<Round> Rounds { get; set; }
    }
}
