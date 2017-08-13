using System;
using System.Collections.Generic;
using System.Text;

namespace TRNMNT.Core.Model
{
    public class CategoryModel
    {
        public Guid CategoryId { get; set; }
        public String Name { get; set; }
        public Guid EventId { get; set; }

        public ICollection<WeightDivisionModel> WeightDivisions { get; set; }
    }
}
