using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Core.Data.Entities
{

    public class WeightDivision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WeightDivisionId { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Descritpion { get; set; }

        public ICollection<Fighter> Fighters {get;set;}
    }
}