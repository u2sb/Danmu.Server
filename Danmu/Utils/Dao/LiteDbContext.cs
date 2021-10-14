using System.IO;
using System.Net;
using Danmu.Models.Configs;
using Danmu.Models.Danmus.DataTables;
using LiteDB;

namespace Danmu.Utils.Dao
{

    public class LiteDbContext
    {
        public LiteDbContext(AppSettings appSettings)
        {
            Database = new LiteDatabase(Path.Combine(appSettings.DanmuDb.Directory, "Danmu.db"));

            var mapper = Database.Mapper;

            // 自定义类型
            mapper.RegisterType<IPAddress>
            (
                serialize: (ip) => ip.ToString(),
                deserialize: (bson) => IPAddress.Parse(bson.AsString ?? string.Empty)
            );

            // 关联
            mapper.Entity<DanmuTable>()
                .DbRef(x => x.Video, "video");

            // 索引
            Database.GetCollection<DanmuTable>().EnsureIndex(x => x.Vid);
            Database.GetCollection<VideoTable>().EnsureIndex(x => x.Vid);
        }

        public LiteDatabase Database { get; }
    }
}
