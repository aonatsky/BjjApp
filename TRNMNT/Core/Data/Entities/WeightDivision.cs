using System;
using System.ComponentModel.DataAnnotations;
namespace TRNMNT.Core.Data.Entities
{

    public class WeightDivision
    {
        [Key]
        public Guid WeightDivisionID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Descritpion { get; set; }
    }
}