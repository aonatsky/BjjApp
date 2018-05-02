using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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

        public async Task RoundComplete(string groupId)
        {
            await Clients.Group(groupId).RoundComplete(groupId);
        }
    }

    public interface IRunEventHubContract : IClientProxy
    {
        Task RefreshRound(BracketModel bracketModel);
        Task RoundStart(RoundModel roundModel);
        Task RoundComplete(string groupId);
    }
}