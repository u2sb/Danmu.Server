using System;
using Microsoft.Extensions.Configuration;

namespace Danmaku.Model
{
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public string[] WithOrigins { get; set; }
        public string[] LiveWithOrigins { get; set; }
        public DanmakuSQL DanmakuSQL { get; set; }
        public Admin Admin { get; set; }
        public string BCookie { get; set; }
    }

    public class DanmakuSQL
    {
        public SqlType Sql { get; set; } = SqlType.SQLite;
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 0;
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DataBase { get; set; }
        public Version Version { get; set; }
    }

    public enum SqlType
    {
        PostgreSQL = 0,
        MySQL = 1,
        SQLite = 2,
        SQLServer = 3
    }

    public class Admin
    {
        public string User { get; set; }
        public string Password { get; set; }
        public int MaxAge { get; set; }
    }
}
