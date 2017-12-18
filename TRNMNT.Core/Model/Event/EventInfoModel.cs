namespace TRNMNT.Core.Model.Event
{
    public class EventModelInfo : EventModelBase
    {
        public string ImgPath { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string TNCFilePath { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string FBLink { get; set; }
        public string VKLink { get; set; }
        public string AdditionalData { get; set; }
        public int EarlyRegistrationPrice { get; set; }
        public int LateRegistrationPrice { get; set; }
        public int EarlyRegistrationPriceForMembers { get; set; }
        public int LateRegistrationPriceForMembers { get; set; }
    }
}
