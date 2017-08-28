using System.Collections.Generic;
using TRNMNT.Core.Model.Category;

namespace TRNMNT.Core.Model.Event
{
    public class EventModel : EventModelBase
    {
        public string ImgPath { get; set; }
        public string UrlPrefix { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string TNCFilePath { get; set; }
        public string CardNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string FBLink { get; set; }
        public string VKLink { get; set; }
        public string AdditionalData { get; set; }

        public virtual ICollection<CategoryModel> CategoryModels { get; set; }
        
    }
}
