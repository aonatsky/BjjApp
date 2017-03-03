using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{

    public class BeltDivision
    {
        [Key]
        public Guid BeltDivisionID { get; set; }
        public String Name { get; set; }
        
    }
}