using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TRNMNT.Data.Entities
{
    public class Round
    {
        [Key]
        public Guid RoundId { get; set; }
        public Guid BracketId { get; set; }
        public Guid FirstParticipantId { get; set; }
        public Guid SecondParticipantId { get; set; }
        public Guid WinnerParticipantId { get; set; }
        public Guid? NextRoundId { get; set; }
        public int Stage { get; set; }

        [JsonIgnore, ForeignKey(nameof(FirstParticipantId))]
        public virtual Participant FirstParticipant { get; set; }

        [JsonIgnore, ForeignKey(nameof(SecondParticipantId))]
        public virtual Participant SecondParticipant { get; set; }

        [JsonIgnore, ForeignKey(nameof(WinnerParticipantId))]
        public virtual Participant WinnerParticipant { get; set; }

        [JsonIgnore]
        public virtual Bracket Bracket { get; set; }

        [JsonIgnore]
        public virtual Round NextRound { get; set; }

    }
}
