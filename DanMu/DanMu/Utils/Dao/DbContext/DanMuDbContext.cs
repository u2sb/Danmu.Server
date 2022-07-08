using System.Net;
using DanMu.Models.DataTables.DanMu;
using DanMu.Models.Settings;
using LiteDB.Async;

namespace DanMu.Utils.Dao.DbContext;

public sealed class DanMuDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ILiteCollectionAsync<DanMuTable> DanMuTable;
    public ILiteCollectionAsync<VideoTable> VideoTable;

    public DanMuDbContext(AppSettings appSettings)
    {
        Database = new LiteDatabaseAsync(Path.Combine(appSettings.DataBase.Directory, appSettings.DataBase.DanMuDb));


        DanMuTable = Database.GetCollection<DanMuTable>("danmu");
        VideoTable = Database.GetCollection<VideoTable>("video");

        // 索引
        DanMuTable.EnsureIndexAsync(x => x.VideoId);
        VideoTable.EnsureIndexAsync(x => x.VideoId);

        var mapper = Database.UnderlyingDatabase.Mapper;

        // 自定义类型
        mapper.RegisterType
        (
            ip => ip.ToString(),
            bson => IPAddress.Parse(bson.AsString ?? string.Empty)
        );

        // 关联
        mapper.Entity<DanMuTable>()
            .DbRef(x => x.Video, "video");
    }

    public new LiteDatabaseAsync Database { get; }
}