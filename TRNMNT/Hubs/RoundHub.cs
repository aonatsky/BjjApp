using System.Threading.Tasks;

namespace TRNMNT.Web.Hubs
{
    public class RoundHub : BaseHub<IRoundHubClient>
    {
        #region Dependencies

        public async Task Send(string data)
        {
            await AllExeptCurrent.Send(data);
        }

        #endregion
    }

    public interface IRoundHubClient
    {
        Task Send(string data);
    }
}
