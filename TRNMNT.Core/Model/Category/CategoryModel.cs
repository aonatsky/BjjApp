using System;
using System.Collections.Generic;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Model.Category
{
    public class CategoryModel : CategoryModelBase
    {
        public Guid EventId { get; set; }
        public int RoundTime { get; set; }
        public ICollection<WeightDivisionModel> WeightDivisionModels { get; set; }
    }
}
