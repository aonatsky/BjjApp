using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Core.Data.Entities
{
    public class Fighter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FighterID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Guid TeamID { get; set; }



        public Team Team { get; set; }
        public Category Category { get; set; }

    }
}