using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Danmaku.Utils.MySql
{
    public class MySqlCon : IMySqlCon
    {
        private readonly string _conStr;

        public MySqlCon(IConfiguration configuration)
        {
            var conf = configuration.GetSection("DanmakuSQL");
            _conStr = $"server={conf["Host"]};userid={conf["UserName"]};password={conf["PassWord"]};database={conf["DataBase"]};";
        }

        public MySqlConnection Connection()
        {
            return new MySqlConnection(_conStr);
        }
    }
}