namespace TRNMNT.Core.Model.Result
{
    public class WebSocketResponse<T>
    {
        public string ConnectionId { get; set; }
        public T Response { get; set; }
    }
}
