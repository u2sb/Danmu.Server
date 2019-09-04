using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using static Danmaku.Model.SqlType;

namespace Danmaku.Model
{
    public class MyDbContext : DbContext
    {
        private readonly DanmakuSQL _sql = AppConfiguration.Config.DanmakuSQL;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_sql.Sql == PostgreSQL)
            {
                _sql.Port = _sql.Port == 0 ? 5432 : _sql.Port;
                var connection =
                    $"Host={_sql.Host};Port={_sql.Port};Database={_sql.DataBase};Username={_sql.UserName};Password={_sql.PassWord};";
                optionsBuilder.UseNpgsql(connection);
            }
//            else if (_sql.Sql == MySQL)
//            {
//                _sql.Port = _sql.Port == 0 ? 3306 : _sql.Port;
//                var connection =
//                    $"Server={_sql.Host};Port={_sql.Port};Database={_sql.DataBase};User={_sql.UserName};Password={_sql.PassWord};Sslmode=none;";
//                optionsBuilder.UseMySql(connection, options => { options.ServerVersion(new Version(8, 0),ServerType.MySql); });
//            }
#if DEBUG
            optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] {new DebugLoggerProvider()}));
#endif
        }
    }


    public class DanmakuContext : MyDbContext
    {
        public DbSet<DanmakuDataBase> Danmaku { get; set; }
    }
}