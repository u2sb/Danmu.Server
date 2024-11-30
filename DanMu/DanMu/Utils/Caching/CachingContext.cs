using DanMu.Models.BiliBili;
using DanMu.Models.Settings;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DanMu.Utils.Caching;

public class CachingContext(DbContextOptions<CachingContext> options) : DbContext(options)
{
  public DbSet<BiliBiliPagesCaching> BiliBiliPagesCaching { get; init; }
  public DbSet<BiliBiliDmCaching> BiliBiliDmCaching { get; init; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<BiliBiliPagesCaching>();
    modelBuilder.Entity<BiliBiliDmCaching>();
  }
}

public static class CachingContextBuilder
{
  public static void Build(DataBase dataBase, DbContextOptionsBuilder option)
  {
    var connectionString = new SqliteConnectionStringBuilder
    {
      Mode = SqliteOpenMode.ReadWriteCreate,
      Cache = SqliteCacheMode.Default,
      DataSource = Path.Combine(dataBase.Directory, dataBase.CachingDb),
      Pooling = true
    }.ToString();
    option.UseSqlite(connectionString);
  }
}