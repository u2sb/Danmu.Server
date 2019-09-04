using Microsoft.Extensions.Configuration;

namespace Danmaku.Model
{
    public class AppConfiguration
    {
        public static AppConfiguration Config { get; set; }

        public AppConfiguration(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public Logging Logging { get; set; }

        public string AllowedHosts { get; set; }
        public string[] WithOrigins { get; set; }
        public int WebSocketPort { get; set; }
        public DanmakuSQL DanmakuSQL { get; set; }
        public string BCookie { get; set; }
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
        public SqlType Sql { get; set; } = 0;
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 0;
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DataBase { get; set; }
    }

    public enum SqlType
    {
        PostgreSQL = 0,
        MySQL = 1,
        SQLite = 2
    }
}