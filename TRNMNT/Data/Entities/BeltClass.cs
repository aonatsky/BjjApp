using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{

    public class BeltClass
    {
        [Key]
        public Guid BeltClassID { get; set; }
        public String Name { get; set; }
        
    }
}