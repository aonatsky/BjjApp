using System.Threading.Tasks;
using TRNMNT.Core.Model.Round;

namespace TRNMNT.Web.Hubs
{
    public class RoundHub : BaseHub<IRoundHubClient>
    {
        #region Dependencies

        public async Task Send(MatchDetailsModel roundDetails)
        {
            await Clients.Group(roundDetails.MatchId).Send(roundDetails);
        }

        #endregion
    }

    public interface IRoundHubClient
    {
        Task Send(MatchDetailsModel roundDetails);
    }
}
