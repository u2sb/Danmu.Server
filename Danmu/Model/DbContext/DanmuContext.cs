using System;
using System.Net;
using Danmu.Model.Config;
using Danmu.Model.Danmu.DanmuData;
using Danmu.Model.DataTable;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Model.DbContext
{
    public class DanmuContext : BaseContext
    {
        public DanmuContext(DbContextOptions<DanmuContext> options) : base(options) { }

        public DbSet<DanmuTable> Danmu { get; set; }
        public DbSet<UserTable> User { get; set; }
        public DbSet<VideoTable> Video { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            switch (Sql.Sql)
            {
                case SqlType.PostgreSql:
                    break;
                case SqlType.SqLite:
                    //弹幕表
                    modelBuilder.Entity<DanmuTable>().Property(e => e.Id).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString(), v => Guid.Parse(v));
                    modelBuilder.Entity<DanmuTable>().Property(e => e.Ip).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmuTable>().Property(e => e.CreateTime).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));
                    modelBuilder.Entity<DanmuTable>().Property(e => e.UpDateTime).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));
                    modelBuilder.Entity<DanmuTable>().Property(e => e.Data).HasColumnType("TEXT")
                                .HasConversion(v => v.ToJson(), v => (BaseDanmuData) v);

                    //用户表
                    modelBuilder.Entity<UserTable>().Property(e => e.CreateTime).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));
                    modelBuilder.Entity<UserTable>().Property(e => e.UpDateTime).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));

                    //视频表
                    modelBuilder.Entity<VideoTable>().Property(e => e.CreateTime).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));
                    modelBuilder.Entity<VideoTable>().Property(e => e.UpDateTime).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString("yyyy-MM-dd hh:mm:ss"), v => DateTime.Parse(v));
                    break;
            }

            modelBuilder.Entity<DanmuTable>().Property(p => p.IsDelete).HasDefaultValue(false);
            modelBuilder.Entity<DanmuTable>().HasIndex(d => new {d.Vid, d.IsDelete});
        }
    }
}
