using System;
namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantRegistrationModel : ParticipantModelBase
    {
        public string UserId { get; set; }
        public Guid TeamId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid WeightDivisionId { get; set; }
        public string PromoCode { get; set; }
        public bool IncludeMembership { get; set; }
        public bool federationMember {get;set;}
    }
}