using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Danmaku.Utils.PostgreSQL
{
    public class PsCon : IPsCon
    {
        private readonly string _conStr;

        public PsCon(IConfiguration configuration)
        {
            var conf = configuration.GetSection("DanmakuSQL");
            _conStr =
                $"Host={conf["Host"]};Username={conf["UserName"]};Password={conf["PassWord"]};Database={conf["DataBase"]};";
        }

        public NpgsqlConnection Connection()
        {
            return new NpgsqlConnection(_conStr);
        }
    }
}