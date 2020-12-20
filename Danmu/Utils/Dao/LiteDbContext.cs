using System.IO;
using Danmu.Models.Configs;
using LiteDB;

namespace Danmu.Utils.Dao
{

    public class LiteDbContext
    {
        public LiteDbContext(DanmuDb db)
        {
            Database = new LiteDatabase(Path.Combine(db.Directory, "Danmu.db"));
        }

        public LiteDatabase Database { get; }
    }
}
