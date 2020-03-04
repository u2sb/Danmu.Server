using System.IO;
using Danmu.Model.Config;
using Danmu.Utils.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Model.DbContext
{
    public class DbContextBuild
    {
        public DbContextBuild(AppConfiguration config, DbContextOptionsBuilder option)
        {
            var sql = config.GetAppSetting().DanmuSql;

            switch (sql.Sql)
            {
                case SqlType.PostgreSql:
                    sql.Port = sql.Port == 0 ? 5432 : sql.Port;
                    option.UseNpgsql(
                            $"Host={sql.Host};Port={sql.Port};Database={sql.DataBase};Username={sql.UserName};Password={sql.PassWord};");
                    break;
                case SqlType.SqLite:
                    if (!Directory.Exists("DataBase")) Directory.CreateDirectory("DataBase");
                    option.UseSqlite("Data Source=DataBase/Danmu.db");
                    break;
            }
        }
    }
}