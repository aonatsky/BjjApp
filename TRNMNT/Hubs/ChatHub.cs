using System.Threading.Tasks;

namespace TRNMNT.Web.Hubs
{
    public class ChatHub : BaseHub<IChatHubClient>
    {
        public async Task Send(string message)
        {
            await Clients.All.Send(message);
            await AllExeptCurrent.Send("Hidden message FROM OWNER ");
            await Current.Send("FROM OTHERS Hidden message");
        }
    }

    public interface IChatHubClient
    {
        Task Send(string message);
    }
}