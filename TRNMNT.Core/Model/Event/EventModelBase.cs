using System;

namespace TRNMNT.Core.Model.Event
{
    public class EventModelBase
    {
        public Guid EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RegistrationStartTS { get; set; }
        public DateTime EarlyRegistrationEndTS { get; set; }
        public DateTime RegistrationEndTS { get; set; }
        public bool BracketsPublished { get; set; }
        public bool ParticipantListsPublished { get; set; }
        public bool CorrectionsEnabled { get; set; }
        public string Title { get; set; }
    }
}
