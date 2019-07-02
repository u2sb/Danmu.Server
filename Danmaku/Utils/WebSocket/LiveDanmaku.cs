using Danmaku.Model;
using Microsoft.Extensions.Configuration;
using WebSocketSharp.Server;

namespace Danmaku.Utils.WebSocket
{
    public class LiveDanmaku : ILiveDanmaku
    {
        private readonly WebSocketServer _webSocketServer;

        public LiveDanmaku(IConfiguration configuration)
        {
            _webSocketServer = new WebSocketServer($"ws://0.0.0.0:{int.Parse(configuration["WebSocketPort"])}");
            _webSocketServer.Start();

            //TODO 临时使用，后面需要写一个管理界面
            _webSocketServer.AddWebSocketService<LiveDanmakuWebSocket>("/live/san");
        }

        public WebSocketServer Server()
        {
            return _webSocketServer;
        }
    }
}