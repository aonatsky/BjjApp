using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers.Interface;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Helpers.Impl
{
    public class PaidServiceFactory : IPaidServiceFactory
    {
        #region Dependencies

        private readonly ITeamService _teamService;
        private readonly IParticipantService _participantService;
        private readonly IFederationMembershipService _federationMembershipService;

        #endregion

        #region .ctor

        public PaidServiceFactory(ITeamService teamService, IParticipantService participantService, IFederationMembershipService federationMembershipService)
        {
            _teamService = teamService;
            _participantService = participantService;
            _federationMembershipService = federationMembershipService;
        }

        #endregion

        #region Public Methods

        public IPaidEntityService GetService(int orderTypeId)
        {
            switch (orderTypeId)
            {
                case (int)OrderTypeEnum.EventParticipation:
                    {
                        return _participantService;
                    }
                case (int)OrderTypeEnum.FederationMembership:
                   {
                       return _federationMembershipService;
                   }
                case (int)OrderTypeEnum.TeamRegistration:
                    {
                        return _teamService;
                    }
                default:
                    {
                        return _participantService;
                    }
            }
        }

        #endregion
    }
}
