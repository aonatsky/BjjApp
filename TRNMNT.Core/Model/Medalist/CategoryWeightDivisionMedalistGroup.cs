using System.Collections.Generic;

namespace TRNMNT.Core.Model.Medalist
{
    public class CategoryWeightDivisionMedalistGroup
    {
        public string CategoryName { get; set; }
        public List<WeightDivisionMedalistGroup> WeightDivisionMedalistGroups { get; set; }
    }
}