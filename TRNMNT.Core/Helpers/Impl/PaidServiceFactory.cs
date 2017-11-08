using TRNMNT.Core.Services;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Helpers
{
    public class PaidServiceFactory : IPaidServiceFactory
    {
        private readonly ITeamService teamService;
        private readonly IParticipantService participantService;

        public PaidServiceFactory(ITeamService teamService, IParticipantService participantService)
        {
            this.teamService = teamService;
            this.participantService = participantService;
        }

        public IPaidEntityService GetService(int orderTypeId)
        {
            switch (orderTypeId)
            {
                case (int)OrderTypeEnum.EventParticipation:
                    {
                        return participantService;
                    }
                //case (int)OrderTypeEnum.FederationMembership:
                //    {
                //        return participantService;
                //    }
                case (int)OrderTypeEnum.TeamRegistration:
                    {
                        return teamService;
                    }
                default:
                    {
                        return participantService;
                    }

            }
        }
    }
}
