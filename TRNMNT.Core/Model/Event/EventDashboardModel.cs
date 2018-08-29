using System.Collections.Generic;

namespace TRNMNT.Core.Model.Event
{
    public class EventDashboardModel : EventModelBase
    {
        public bool BracketsCreated { get; set; }
        public string EventStatus { get; set; }
        public List<CategoryWeightDivisionParticipants> ParticipantGroups { get; set; }
    }

    public class CategoryWeightDivisionParticipants
    {
        public string CategoryName { get; set; }
        public string WeightDivisionName { get; set; }
        public int ParticipantsCount { get; set; }
    }
}