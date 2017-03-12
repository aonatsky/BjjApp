using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{
    public class Category        
    {
        [Key]
        public int CategoryID { get; set; }
        public String Name { get; set; }
        
        public ICollection<Fighter> Fighter {get;set;}

    }
}