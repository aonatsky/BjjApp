using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Data.Entities
{

    public class WeightDivision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WeightDivisionId { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Descritpion { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Fighter> Fighters {get;set;}
    }
}