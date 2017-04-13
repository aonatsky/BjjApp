using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Core.Data.Entities
{
    public class Fighter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FighterId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Guid TeamId { get; set; }



        public Team Team { get; set; }
        public Category Category { get; set; }

    }
}