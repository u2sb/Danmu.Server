using Danmaku.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Danmaku.Utils.PostgreSQL
{
    public class PsCon : IPsCon
    {
        private readonly string _conStr;

        public PsCon(IConfiguration configuration)
        {
            var dansql = new AppConfiguration(configuration).DanmakuSQL;
            _conStr =
                $"Host={dansql.Host};Username={dansql.UserName};Password={dansql.PassWord};Database={dansql.DataBase};";
        }

        public NpgsqlConnection Connection()
        {
            return new NpgsqlConnection(_conStr);
        }
    }
}