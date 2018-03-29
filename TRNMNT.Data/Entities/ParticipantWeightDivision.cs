using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class ParticipantWeightDivision
    {
        [Key]
        [Column(Order = 1)]
        public Guid WeightDivisionId { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ParticipantId { get; set; }

        [JsonIgnore]
        public virtual Participant Participant { get; set; }

        [JsonIgnore]
        public virtual WeightDivision WeightDivision { get; set; }

    }
}