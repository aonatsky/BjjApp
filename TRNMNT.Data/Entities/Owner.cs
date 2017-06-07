using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{
    public class Owner
    {
        [Key]
        public Guid OwnerId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public ICollection<Tournament> Tournaments { get; set; }

    }
}