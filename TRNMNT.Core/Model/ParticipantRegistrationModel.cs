using System;

namespace TRNMNT.Core.Model
{
    public class ParticipantRegistrationModel
    {
        public Guid EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserId { get; set; }
        public string Team { get; set; }
        public string CategoryId { get; set; }
        public string WeightDivisionId { get; set; }
    }
}
