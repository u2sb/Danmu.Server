using Microsoft.Extensions.Configuration;

namespace Danmu.Model.Config
{
    public class AppSettings
    {
        public AppSettings() { }

        public AppSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public KestrelSettings KestrelSettings { get; set; }
        public string[] WithOrigins { get; set; }
        public string[] LiveWithOrigins { get; set; }
        public string[] AdminWithOrigins { get; set; }
        public DanmuSql DanmuSql { get; set; }
        public Admin Admin { get; set; }
        public string BCookie { get; set; }
    }

    public class DanmuSql
    {
        public SqlType Sql { get; set; } = SqlType.SqLite;
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 0;
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DataBase { get; set; }
        public int PoolSize { get; set; } = 8;
    }

    public enum SqlType
    {
        PostgreSql = 0,
        SqLite = 1
    }

    public class Admin
    {
        public string User { get; set; }
        public string Password { get; set; }
        public int MaxAge { get; set; }
    }
}