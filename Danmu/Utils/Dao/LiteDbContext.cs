using System.IO;
using Danmu.Models.Configs;
using LiteDB;

namespace Danmu.Utils.Dao
{

    public class LiteDbContext
    {
        public LiteDbContext(AppSettings appSettings)
        {
            Database = new LiteDatabase(Path.Combine(appSettings.DanmuDb.Directory, "Danmu.db"));
        }

        public LiteDatabase Database { get; }
    }
}
