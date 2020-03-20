using Danmu.Utils.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Danmu.Model.DbContext
{
    public class DbContextBuild
    {
        public DbContextBuild(AppConfiguration config, DbContextOptionsBuilder option)
        {
            var sql = config.GetAppSetting().DanmuSql;
            sql.Port = sql.Port == 0 ? 5432 : sql.Port;
            option.UseNpgsql(
                    $"Host={sql.Host};Port={sql.Port};Database={sql.DataBase};Username={sql.UserName};Password={sql.PassWord};");
#if DEBUG
            option.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
#endif
        }
    }
}
