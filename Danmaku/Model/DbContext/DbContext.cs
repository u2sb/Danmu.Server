using System.IO;
using Danmaku.Utils.AppConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using static Danmaku.Model.SqlType;

namespace Danmaku.Model.DbContext
{
    public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private protected readonly DanmakuSQL Sql;

        public MyDbContext(IAppConfiguration configuration)
        {
            Sql = configuration.GetAppSetting().DanmakuSQL;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (Sql.Sql)
            {
                case PostgreSQL:
                    Sql.Port = Sql.Port == 0 ? 5432 : Sql.Port;
                    optionsBuilder.UseNpgsql(
                            $"Host={Sql.Host};Port={Sql.Port};Database={Sql.DataBase};Username={Sql.UserName};Password={Sql.PassWord};");
                    break;
                case MySQL:
                    Sql.Port = Sql.Port == 0 ? 3306 : Sql.Port;
                    optionsBuilder.UseMySql(
                            $"Server={Sql.Host};Port={Sql.Port};Database={Sql.DataBase};UserId={Sql.UserName};Password={Sql.PassWord};",
                            mySqlDbContextOptionsBuilder =>
                            {
                                mySqlDbContextOptionsBuilder.ServerVersion(Sql.Version, ServerType.MySql);
                            });
                    break;
                case SQLite:
                    if (!Directory.Exists("DataBase")) Directory.CreateDirectory("DataBase");
                    optionsBuilder.UseSqlite("Data Source=DataBase/Danmaku.db");
                    break;
            }

#if DEBUG
            optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] {new DebugLoggerProvider()}));
#endif
        }
    }
}