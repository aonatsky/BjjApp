using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TRNMNT.Data.Entities
{
    class Participant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ParticipantId { get; set; }
        public string UserId{ get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

    }
}
