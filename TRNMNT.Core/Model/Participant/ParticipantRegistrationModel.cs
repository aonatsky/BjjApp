using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantRegistrationModel : ParticipantModelBase
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public string TeamId { get; set; }
        public string CategoryId { get; set; }
        public string WeightDivisionId { get; set; }
        public string PromoCode { get; set; }
    }
}
