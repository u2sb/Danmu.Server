using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using static Danmaku.Model.SqlType;

namespace Danmaku.Model
{
    public class MyDbContext : DbContext
    {
        private protected readonly DanmakuSQL Sql = AppConfiguration.Config.DanmakuSQL;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (Sql.Sql)
            {
                case PostgreSQL:
                    Sql.Port = Sql.Port == 0 ? 5432 : Sql.Port;
                    var connection0 =
                        $"Host={Sql.Host};Port={Sql.Port};Database={Sql.DataBase};Username={Sql.UserName};Password={Sql.PassWord};";
                    optionsBuilder.UseNpgsql(connection0);
                    break;
//                case MySQL:
//                    Sql.Port = Sql.Port == 0 ? 3306 : Sql.Port;
//                    var connection1 =
//                        $"Server={Sql.Host};Port={Sql.Port};Database={Sql.DataBase};UserId={Sql.UserName};Password={Sql.PassWord};Sslmode=none;";
//                    optionsBuilder.UseMySql(connection1, optionsBuilder =>
//                        {
//                            optionsBuilder.ServerVersion(new Version(8, 0, 16), ServerType.MySql);
//                        });
//                    break;
                case SQLite:
                    var connection2 =
                        "Data Source=Danmaku.db";
                    optionsBuilder.UseSqlite(connection2);
                    break;
            }

#if DEBUG
            optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] {new DebugLoggerProvider()}));
#endif
        }
    }


    public class DanmakuContext : MyDbContext
    {
        public DbSet<DanmakuDataBase> Danmaku { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (Sql.Sql)
            {
                case PostgreSQL:
                    modelBuilder.Entity<DanmakuDataBase>().HasIndex(d => d.Vid).HasMethod("hash");
                    break;
//                case MySQL:
//                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Ip)
//                        .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
//                    break;
                case SQLite:
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Ip)
                        .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.DanmakuData)
                        .HasConversion(v => v.ToJson(), v => new DanmakuData(v));
                    break;
            }
        }
    }
}