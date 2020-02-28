using System;
using System.Net;
using Danmaku.Model.Danmaku;
using Danmaku.Utils.AppConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Danmaku.Model.DbContext
{
    public class DanmakuContext : MyDbContext
    {
        public DanmakuContext(IAppConfiguration configuration) : base(configuration) { }

        public DbSet<DanmakuDataBase> Danmaku { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (Sql.Sql)
            {
                case SqlType.PostgreSQL:
                    break;
                case SqlType.MySQL:
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Id).HasColumnType("varchar(40)")
                                .HasConversion(v => v.ToString(), v => Guid.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Ip).HasColumnType("varchar(40)")
                                .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.DanmakuData).HasColumnType("json")
                                .HasConversion(v => v.ToJson(), v => DanmakuData.FromJson(v));
                    break;
                case SqlType.SQLite:
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Id).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString(), v => Guid.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Ip).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.Date).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));
                    modelBuilder.Entity<DanmakuDataBase>().Property(e => e.DanmakuData).HasColumnType("TEXT")
                                .HasConversion(v => v.ToJson(), v => DanmakuData.FromJson(v));
                    break;
            }

            modelBuilder.Entity<DanmakuDataBase>().Property(p => p.IsDelete).HasDefaultValue(false);
            modelBuilder.Entity<DanmakuDataBase>().HasIndex(d => new {d.Vid, d.IsDelete});
        }
    }
}