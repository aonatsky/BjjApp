using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Core.Data.Entities
{

    public class AgeDivision
    {
        [Key]
        public Guid AgeDivisionID { get; set; }
        public String Name { get; set; }

        public int Age {get;set;}
    }
}