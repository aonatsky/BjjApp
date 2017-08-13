using System;
using System.Collections.Generic;
using System.Text;

namespace TRNMNT.Core.Model
{
    public class EventModel
    {
        public Guid EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RegistrationStartTS { get; set; }
        public DateTime RegistrationEndTS { get; set; }
        public string ImgPath { get; set; }
        public string Title { get; set; }
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

        public virtual ICollection<CategoryModel> Categories { get; set; }
        
    }
}
