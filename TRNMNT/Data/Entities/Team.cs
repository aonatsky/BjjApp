using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{
    public class Team
    {
        [Key]
        public Guid TeamID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public ICollection<Fighter> Fighters { get; set; }

    }
}