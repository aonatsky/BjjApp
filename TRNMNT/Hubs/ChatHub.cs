using System;
using System.Threading.Tasks;

namespace TRNMNT.Web.Hubs
{
    public class ChatHub : BaseHub<IChatHubClient>
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync(message);
        }
    }

    public interface IChatHubClient : IHubClient
    {
        Task SendAsync(string message);
    }
}