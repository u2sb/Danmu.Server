using Danmaku.Model;
using Microsoft.Extensions.Configuration;
using WebSocketSharp.Server;

namespace Danmaku.Utils.WebSocket
{
    public class LiveDanmaku : ILiveDanmaku
    {
        private readonly HttpServer _webSocketServer;
        private readonly AppConfiguration _appConfiguration;

        public LiveDanmaku(IConfiguration configuration)
        {
            _appConfiguration = new AppConfiguration(configuration);
            _webSocketServer = new HttpServer(_appConfiguration.WebSocketPort);
            _webSocketServer.Start();

            //TODO 临时使用，后面需要写一个管理界面
            _webSocketServer.AddWebSocketService<LiveDanmakuWebSocket>("/live/san");
        }

        public HttpServer Server()
        {
            return _webSocketServer;
        }
    }
}