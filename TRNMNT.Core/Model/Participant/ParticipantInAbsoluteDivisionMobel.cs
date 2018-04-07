using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantInAbsoluteDivisionMobel
    {
        public Guid ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public string WeightDivisionName { get; set; }
        public bool IsSelectedIntoDivision { get; set; }
    }
}