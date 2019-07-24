using Microsoft.Extensions.Configuration;

namespace Danmaku.Model
{
    public class AppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public Logging Logging { get; set; }

        public string AllowedHosts { get; set; }
        public string[] WithOrigins { get; set; }
        public int WebSocketPort { get; set; }
        public DanmakuSQL DanmakuSQL { get; set; }
    }


    public class Logging
    {
        public class LogLevel
        {
            public string Default { get; set; }
        }
    }

    public class DanmakuSQL
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DataBase { get; set; }
    }
}