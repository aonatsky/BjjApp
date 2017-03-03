using System;
using System.ComponentModel.DataAnnotations;

namespace TRNMNT.Data.Entities
{

    public class AgeClass
    {
        [Key]
        public Guid AgeClassID { get; set; }
        public String Name { get; set; }

        public int Age {get;set;}
    }
}