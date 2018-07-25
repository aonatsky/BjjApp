using System;

namespace TRNMNT.Core.Model.WeightDivision
{
    public class WeightDivisionModel : WeightDivisionModelBase
    {
        public int Weight { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public int Status { get; set; }
    }
}
