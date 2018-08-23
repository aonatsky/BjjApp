namespace TRNMNT.Core.Model.User
{
    public class UserModelAthlete : UserModelBase
    {
        public string TeamMembershipApprovalStatus { get; set; }
        public string TeamName { get; set; }
        public bool IsFederationMember {get;set;}
        public bool IsParticipant {get;set;}
        public bool IsTeamOwner {get;set;}
    }
}