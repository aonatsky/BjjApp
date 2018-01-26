using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TRNMNT.Web.WebSockets.Handlers
{
    public class TestWebSocketHandler : WebSocketHandler
    {
        public TestWebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            await SendMessageToAllAsync(message);
        }
    }
}