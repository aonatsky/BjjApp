using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRNMNT.Core.Data.Entities
{
    public class Category        
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        public String Name { get; set; }
        
        public ICollection<Fighter> Fighter {get;set;}

    }
}