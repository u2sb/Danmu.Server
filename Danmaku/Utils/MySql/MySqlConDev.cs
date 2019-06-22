using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Danmaku.Utils.MySql
{
    public class MySqlConDev : IMySqlCon
    {
        private readonly string _conStr;


        public MySqlConDev()
        {
            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build()
                .GetSection("DanmakuSQL");
            _conStr =
                $"server={conf["Host"]};userid={conf["UserName"]};password={conf["PassWord"]};database={conf["DataBase"]};";
        }

        public MySqlConnection Connection()
        {
            return new MySqlConnection(_conStr);
        }
    }
}