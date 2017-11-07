using System.Collections.Generic;
using TRNMNT.Core.Model.Category;

namespace TRNMNT.Core.Model.Event
{
    public class EventModelFull : EventModelInfo
    {
        
        public string UrlPrefix { get; set; }
        public bool PromoCodeEnabled { get; set; }
        public string PromoCodeListPath { get; set; }
        public virtual ICollection<CategoryModel> CategoryModels { get; set; }
        
    }
}
