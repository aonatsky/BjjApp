using System.Threading.Tasks;
using TRNMNT.Core.Model.Round;

namespace TRNMNT.Web.Hubs
{
    public class RoundHub : BaseHub<IRoundHubClient>
    {
        #region Dependencies

        public async Task Send(RoundDetailsModel roundDetails)
        {
            await Clients.Group(roundDetails.RoundId).Send(roundDetails);
        }

        #endregion
    }

    public interface IRoundHubClient
    {
        Task Send(RoundDetailsModel roundDetails);
    }
}
