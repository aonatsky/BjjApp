using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Web.Hubs
{
    public abstract class BaseHub<T> : Hub<T> where T : class, IHubClient
    {

        protected WebSocketResponse<TResponse> Response<TResponse>(TResponse response)
        {
            return new WebSocketResponse<TResponse>
            {
                ConnectionId = Context.ConnectionId,
                Response = response
            };
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.AllExcept(new[] { Context.ConnectionId }).ClientConnectedAsync(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.AllExcept(new[] { Context.ConnectionId }).ClientDisconnectedAsync(Context.ConnectionId, exception);
            await base.OnDisconnectedAsync(exception);
        }
    }
}