using Microsoft.EntityFrameworkCore;

namespace DanMu.Utils.Dao.DbContext;

public sealed class DanMuDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DanMuDbContext(DbContextOptions<DanMuDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}