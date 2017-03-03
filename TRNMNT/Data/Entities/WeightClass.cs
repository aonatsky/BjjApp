using System;
using System.ComponentModel.DataAnnotations;
namespace TRNMNT.Data.Entities
{

    public class WeightClass
    {
        [Key]
        public Guid WeightClassID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Descritpion { get; set; }
    }
}