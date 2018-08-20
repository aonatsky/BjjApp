using System;

namespace TRNMNT.Core.Model.Participant
{
    public class ParticipantTableModel : ParticipantModelBase
    {
        public string UserId { get; set; }
        public string CategoryName { get; set; }
        public string WeightDivisionName { get; set; }
        public Guid? TeamId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid WeightDivisionId { get; set; }
        public bool IsMember { get; set; }
        public string ApprovalStatus { get; set; }
        public string WeightInStatus { get; set; }
    }
}