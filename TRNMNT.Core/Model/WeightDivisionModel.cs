using System;

namespace TRNMNT.Core.Model
{
    public class WeightDivisionModel
    {
        public Guid WeightDivisionId { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Descritpion { get; set; }
        public string CategoryId { get; set; }
    }
}
