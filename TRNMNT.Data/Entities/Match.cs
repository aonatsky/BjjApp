using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Match
    {
        [Key]
        public Guid MatchId { get; set; }
        public Guid WeightDivisionID { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? AParticipantId { get; set; }
        public Guid? BParticipantId { get; set; }
        public Guid? WinnerParticipantId { get; set; }
        public Guid? NextMatchId { get; set; }
        public int Stage { get; set; }
        public int Order { get; set; }
        public int MatchType { get; set; }
        public int MatchResultType { get; set; }
        public string RoundResultDetails { get; set; }

        [JsonIgnore, ForeignKey(nameof(AParticipantId))]
        public virtual Participant AParticipant { get; set; }

        [JsonIgnore, ForeignKey(nameof(BParticipantId))]
        public virtual Participant BParticipant { get; set; }

        [JsonIgnore, ForeignKey(nameof(WinnerParticipantId))]
        public virtual Participant WinnerParticipant { get; set; }

        [JsonIgnore]
        public virtual WeightDivision WeightDivision { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public virtual Match NextMatch { get; set; }

    }
}
