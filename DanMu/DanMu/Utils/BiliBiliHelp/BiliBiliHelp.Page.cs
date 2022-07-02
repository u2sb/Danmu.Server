using System.Text.Json;
using DanMu.Models.BiliBili;

namespace DanMu.Utils.BiliBiliHelp;

public partial class BiliBiliHelp
{
    /// <summary>
    ///     获取Cid和时长
    /// </summary>
    /// <param name="aid">视频的aid</param>
    /// <param name="p">分p</param>
    /// <returns>cid</returns>
    public async Task<CidAndDuration?> GetCidAndDurationAsync(int aid, int p)
    {
        var pages = await GetBiliBiliPageAsync(aid);

        return pages == null ? null : GetCidAndDuration(pages, p);
    }

    /// <summary>
    ///     获取Cid和时长
    /// </summary>
    /// <param name="bvid"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public async Task<CidAndDuration?> GetCidAndDurationAsync(string bvid, int p)
    {
        var pages = await GetBiliBiliPageAsync(bvid);
        return pages == null ? null : GetCidAndDuration(pages, p);
    }

    /// <summary>
    ///     获取视频Cid和分P信息
    /// </summary>
    /// <param name="bvid"></param>
    /// <returns></returns>
    public async Task<BPage?> GetBiliBiliPageAsync(string bvid)
    {
        //使用缓存
        return await _caching.GetAsync($"{nameof(GetBiliBiliPageAsync)}{bvid}",
            async () =>
            {
                var a = await GetBiliBiliDataRawAsync(PageUrl, new Dictionary<string, string> { { "bvid", bvid } });
                if (a is { Length: > 0 })
                    return await JsonSerializer.DeserializeAsync<BPage>(new MemoryStream(a));
                return null;
            }, TimeSpan.FromHours(_setting.PageCacheTime));
    }

    /// <summary>
    ///     获取视频Cid和分P信息
    /// </summary>
    /// <param name="aid">视频的aid</param>
    /// <returns>Page数据</returns>
    public async Task<BPage?> GetBiliBiliPageAsync(int aid)
    {
        return await _caching.GetAsync($"{nameof(GetBiliBiliPageAsync)}{aid}",
            async () =>
            {
                var a = await GetBiliBiliDataRawAsync(PageUrl,
                    new Dictionary<string, string> { { "aid", aid.ToString() } });
                if (a is { Length: > 0 })
                    return await JsonSerializer.DeserializeAsync<BPage>(new MemoryStream(a));
                return null;
            }, TimeSpan.FromHours(_setting.PageCacheTime));
    }

    /// <summary>
    ///     获取Cid
    /// </summary>
    /// <param name="pages">Page数据</param>
    /// <param name="p">分p</param>
    /// <returns>cid</returns>
    private CidAndDuration? GetCidAndDuration(BPage pages, int p)
    {
        if (pages.Data != null)
        {
            var a = pages.Code == 0
                ? pages.Data.Where(e => e.Page == p).Select(s => new CidAndDuration
                {
                    Cid = s.Cid,
                    Duration = s.Duration
                }).FirstOrDefault()
                : null;

            return a;
        }

        return null;
    }
}