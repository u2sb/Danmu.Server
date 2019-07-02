using WebSocketSharp.Server;

namespace Danmaku.Utils.WebSocket
{
    public interface ILiveDanmaku
    {
        WebSocketServer Server();
    }
}