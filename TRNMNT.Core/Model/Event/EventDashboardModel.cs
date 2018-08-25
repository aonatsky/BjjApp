using System.Collections.Generic;

namespace TRNMNT.Core.Model.Event
{
    public class EventDashboardModel : EventModelBase
    {
        public bool CorrectionsEnabled { get; set; }
        public bool BracketsCreated { get; set; }
        public bool BracketsPublished { get; set; }
        public bool ParticipantListsPublished { get; set; }
        public string EventStatus { get; set; }
        public List<CategoryWeightDivisionParticipants> ParticipantGroups {get;set;} 
    }

    public class CategoryWeightDivisionParticipants
    {
        public string CategoryName { get; set; }
        public string WeightDivisionName { get; set; }
        public int ParticipantsCount { get; set; }
    }
}