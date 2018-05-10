using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Web.Hubs
{
    public class RunEventHub : BaseHub<IRunEventHubContract>
    {
        private readonly IBracketService _bracketService;

        public RunEventHub(IBracketService bracketService)
        {
            _bracketService = bracketService;
        }

        public async Task RoundStart(RoundModel roundModel)
        {
            await Clients.Group(roundModel.WeightDivisionId.ToString()).RoundStart(roundModel);
        }

        public async Task RoundComplete(Guid weightDivisionId)
        {
            var divisionId = weightDivisionId.ToString();
            var bracketModel = await _bracketService.GetBracketModelAsync(weightDivisionId);
            if (bracketModel == null)
            {
				// todo add logging
				return;
			}
            var refreshModel = new RefreshBracketModel
            {
                WeightDivisionId = divisionId,
                Bracket = bracketModel
            };
            await ToGroup(divisionId).RoundComplete(refreshModel);
        }
    }

    public interface IRunEventHubContract
    {
        Task RoundStart(RoundModel roundModel);
        Task RoundComplete(RefreshBracketModel refreshModel);

    }
}