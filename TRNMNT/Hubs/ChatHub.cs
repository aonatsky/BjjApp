using System.Threading.Tasks;

namespace TRNMNT.Web.Hubs
{
    public class ChatHub : BaseHub<IChatHubContract>
    {
        public async Task Send(string message)
        {
            await Clients.All.Send(message);
            await AllExeptCurrent.Send("Hidden message FROM OWNER ");
            await Current.Send("FROM OTHERS Hidden message");
        }
    }

    public interface IChatHubContract
    {
        Task Send(string message);
    }
}