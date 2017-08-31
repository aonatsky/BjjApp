using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantModelBase
    {
        public Guid EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
