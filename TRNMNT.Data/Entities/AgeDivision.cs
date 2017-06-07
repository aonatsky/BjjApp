using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{

    public class AgeDivision
    {
        [Key]
        public Guid AgeDivisionId { get; set; }
        public String Name { get; set; }

        public int Age {get;set;}
    }
}