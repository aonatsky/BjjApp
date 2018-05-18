using System;

namespace TRNMNT.Core.Model.Participant
{
    public abstract class ParticipantModelBase 
    {
        public Guid ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TeamName { get; set; }
    }
}
