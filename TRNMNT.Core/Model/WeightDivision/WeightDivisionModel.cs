using System;

namespace TRNMNT.Core.Model.WeightDivision
{
    public class WeightDivisionModel : WeightDivisionModelBase
    {
        public int Weight { get; set; }
        public string Descritpion { get; set; }
        public Guid CategoryId { get; set; }
    }
}
