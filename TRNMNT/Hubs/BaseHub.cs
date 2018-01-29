using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Web.Hubs
{
    public abstract class BaseHub<T> : Hub<T> where T : class
    {
        protected WebSocketResponse<TResponse> Response<TResponse>(TResponse response)
        {
            return new WebSocketResponse<TResponse>
            {
                ConnectionId = Context.ConnectionId,
                Response = response
            };
        }

        public virtual async Task JoinGroupAsync(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        protected T AllExeptCurrent => Clients.AllExcept(new[] {Context.ConnectionId});

        protected T Current => Clients.Client(Context.ConnectionId);

        protected T ToGroup(string groupName)
        {
            return Clients.Group(groupName);
        }

        public override async Task OnConnectedAsync()
        {
            if (this is Hub hub)
            {
                await hub.Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("ClientConnected", Context.ConnectionId);
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (this is Hub hub)
            {
                await hub.Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("ClientDisconnected", Context.ConnectionId, exception);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}