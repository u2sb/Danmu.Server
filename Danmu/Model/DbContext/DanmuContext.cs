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
                    modelBuilder.Entity<DanmuTable>().Property(e => e.Ip).HasColumnType("TEXT")
                                .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
                    modelBuilder.Entity<DanmuTable>().Property(e => e.Data).HasColumnType("TEXT")
                                .HasConversion(v => v.ToJson(), v => (BaseDanmuData) v);

                    //视频表
                    modelBuilder.Entity<VideoTable>().Property(e => e.Referer).HasColumnType("TEXT")
                                .HasConversion(v => v.ToJson(), v => Referer.FromJson(v));
                    break;
            }

            modelBuilder.Entity<DanmuTable>().Property(p => p.IsDelete).HasDefaultValue(false);
            modelBuilder.Entity<DanmuTable>().HasIndex(d => new {d.Vid, d.IsDelete});
        }
    }
}
