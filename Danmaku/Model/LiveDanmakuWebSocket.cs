using WebSocketSharp;
using WebSocketSharp.Server;

namespace Danmaku.Model
{
    public class LiveDanmakuWebSocket:WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(e.Data);
        }
    }
}