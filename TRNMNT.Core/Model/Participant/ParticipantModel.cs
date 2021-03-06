using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantModel
    {
        public Guid ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string WeightDivision { get; set; }
        public string Team { get; set; }
        public string Category { get; set; }
        public string IsMember { get; set; }
    }

}