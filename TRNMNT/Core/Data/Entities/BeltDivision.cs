using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{

    public class BeltDivision
    {
        [Key]
        public Guid BeltDivisionID { get; set; }
        public String Name { get; set; }
        
    }
}