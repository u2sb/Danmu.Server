using DanMu.Models.DanMu.Generic.V1;
using DanMu.Models.DataTables.DanMu;
using DanMu.Utils.Dao.DbContext;

using LiteDB.Async;

using System.Net;

namespace DanMu.Utils.Dao.Danmu;

public class DanmuTableDao
{
    private readonly ILiteCollectionAsync<DanMuTable> _danmuTable;
    private readonly ILiteCollectionAsync<VideoTable> _videoTable;

    public DanmuTableDao(DanMuDbContext dbContext)
    {
        _danmuTable = dbContext.DanMuTable;
        _videoTable = dbContext.VideoTable;
    }

    /// <summary>
    ///     通过VID查询弹幕
    /// </summary>
    /// <param name="vid"></param>
    /// <returns></returns>
    public async Task<GenericDanMu[]> GetDanmuByVidAsync(string vid)
    {
        return await _danmuTable.Query().Where(w => w.VideoId == vid).ToArrayAsync();
    }

    /// <summary>
    ///     插入弹幕
    /// </summary>
    /// <param name="danmu"></param>
    /// <param name="vid"></param>
    /// <param name="pageUrl"></param>
    /// <param name="ip"></param>
    /// <returns></returns>
    public async Task<bool> InsertDanMuAsync(GenericDanMu danmu, string vid, string? pageUrl, IPAddress? ip)
    {
        // 先处理Video表
        var video = await _videoTable.Query().Where(w => w.VideoId == vid).FirstOrDefaultAsync();
        if (video == null)
        {
            video = new VideoTable
            {
                VideoId = vid
            };

            if (pageUrl != null && string.IsNullOrWhiteSpace(pageUrl)) video.PageUrl.Add(pageUrl);

            await _videoTable.InsertAsync(video);
        }
        else
        {
            if (pageUrl != null && video.PageUrl.IndexOf(pageUrl) < 0)
            {
                video.PageUrl.Add(pageUrl);
                await _videoTable.UpdateAsync(video);
            }
        }

        // 再处理弹幕表
        var d = new DanMuTable(danmu)
        {
            VideoId = vid,
            Video = video,
            Ip = ip
        };

        return await _danmuTable.InsertAsync(d) != null;
    }

    /// <summary>
    ///     获取全部弹幕
    /// </summary>
    /// <returns></returns>
    public async Task<DanMuTable[]> GetAllDanMuAsync()
    {
        return await _danmuTable.Query().Include(x => x.Video).OrderByDescending(e => e.CTime).ToArrayAsync();
    }

    /// <summary>
    ///     删除弹幕
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public async Task<bool> DeleteDanMuAsync(Guid guid)
    {
        var a = await _danmuTable.DeleteManyAsync(e => e.Id == guid);
        return a > 0;
    }

    /// <summary>
    /// 编辑弹幕
    /// </summary>
    /// <param name="danmu"></param>
    /// <returns></returns>
    public async Task<bool> EditorDanMuAsync(DanMuTable danmu)
    {
        return await _danmuTable.UpdateAsync(danmu);
    }
}