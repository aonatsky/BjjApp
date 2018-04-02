using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantSmallTableMobel
    {
        public Guid ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public string WeightDivisionName { get; set; }
    }
}