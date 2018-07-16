using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantRegistrationModel : ParticipantModelBase
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public Guid TeamId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid WeightDivisionId { get; set; }
        public string PromoCode { get; set; }
    }
}
