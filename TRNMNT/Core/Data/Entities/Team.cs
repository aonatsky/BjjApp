using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{
    public class Team
    {
        [Key]
        public Guid TeamId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public ICollection<Fighter> Fighters { get; set; }

    }
}