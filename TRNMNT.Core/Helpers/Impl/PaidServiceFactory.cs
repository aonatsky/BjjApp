using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers.Interface;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Helpers.Impl
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
