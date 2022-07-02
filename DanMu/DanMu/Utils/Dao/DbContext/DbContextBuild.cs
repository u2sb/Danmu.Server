using DanMu.Models.Settings;
using MaikeBing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DanMu.Utils.Dao.DbContext;

public class DbContextBuild
{
    public DbContextBuild(DataBase config, DbContextOptionsBuilder option)
    {
        option.UseLiteDB(Path.Combine(config.Directory, config.DanMuDb));
        // #if DEBUG
        //         option.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
        // #endif
    }
}