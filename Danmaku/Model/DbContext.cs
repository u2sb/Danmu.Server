using System;
using System.IO;
using System.Net;
using Danmaku.Utils.AppConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using static Danmaku.Model.SqlType;

namespace Danmaku.Model
{
    public class MyDbContext : DbContext
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
                    optionsBuilder.UseNpgsql($"Host={Sql.Host};Port={Sql.Port};Database={Sql.DataBase};Username={Sql.UserName};Password={Sql.PassWord};");
                    break;
                case MySQL:
                    Sql.Port = Sql.Port == 0 ? 3306 : Sql.Port;
                    optionsBuilder.UseMySql($"Server={Sql.Host};Port={Sql.Port};Database={Sql.DataBase};UserId={Sql.UserName};Password={Sql.PassWord};", mySqlDbContextOptionsBuilder =>
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


    public class DanmakuContext : MyDbContext
    {
        public DanmakuContext(IAppConfiguration configuration) : base(configuration)
        {
        }
        public DbSet<DanmakuDataBase> Danmaku { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (Sql.Sql)
            {
                case PostgreSQL:
                    break;
                case MySQL:
	                modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Id).HasColumnType("varchar(40)").HasConversion(v => v.ToString(), v => Guid.Parse(v));
          modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Ip).HasColumnType("varchar(40)")
                        .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.DanmakuData).HasColumnType("json")
                        .HasConversion(v => v.ToJson(), v => DanmakuData.FromJson(v));
                    break;
                case SQLite:
	                modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Id).HasColumnType("TEXT").HasConversion(v => v.ToString(), v => Guid.Parse(v));
	                modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Ip)
                        .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.DanmakuData).HasColumnType("TEXT")
                        .HasConversion(v => v.ToJson(), v => DanmakuData.FromJson(v));
                    break;
            }
            modelBuilder.Entity<DanmakuDataBase>().Property(p => p.IsDelete).HasDefaultValue(false);
            modelBuilder.Entity<DanmakuDataBase>().HasIndex(d => new { d.Vid, d.IsDelete });
        }
    }
}