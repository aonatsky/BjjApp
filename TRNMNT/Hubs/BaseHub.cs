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

        public async Task<bool> JoinGroup(string groupName)
        {
            try
            {
                await JoinGroupAsync(groupName);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<bool> LeaveGroup(string groupName)
        {
            try
            {
                await LeaveGroupAsync(groupName);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        protected virtual async Task JoinGroupAsync(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        protected virtual async Task LeaveGroupAsync(string groupName)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }

        protected T AllExeptCurrent => Clients.AllExcept(new[] {Context.ConnectionId});

        protected T Current => Clients.Client(Context.ConnectionId);

        protected T ToGroup(object groupName)
        {
            return Clients.Group(groupName.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            if (this is Hub hub)
            {
                await hub.Clients.Client(Context.ConnectionId).InvokeAsync("Connected", Context.ConnectionId);
                await hub.Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("OtherClientConnected", Context.ConnectionId);
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (this is Hub hub)
            {
                await hub.Clients.Client(Context.ConnectionId).InvokeAsync("Disconnected", Context.ConnectionId);
                await hub.Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("OtherClientDisconnected", Context.ConnectionId, exception);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}