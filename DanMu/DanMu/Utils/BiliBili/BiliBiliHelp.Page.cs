using System.Text.Json;
using DanMu.Models.BiliBili;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp
{
  /// <summary>
  ///   获取视频详细
  /// </summary>
  /// <param name="bvid"></param>
  /// <returns></returns>
  private ValueTask<BiliBiliPages?> GetBiliBiliPageAsync(string bvid)
  {
    //使用缓存
    return caching.PagesGetOrSetAsync(bvid, async () =>
    {
      var a = await GetBiliBiliDataRawAsync(PageUrl, new Dictionary<string, string> { { "bvid", bvid } }).ConfigureAwait(false);
      if (a != null)
        return await JsonSerializer.DeserializeAsync<BiliBiliPages>(a).ConfigureAwait(false);
      return null;
    }, TimeSpan.FromHours(_setting.PageCacheTime));
  }

  /// <summary>
  ///   获取分P详细信息
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async ValueTask<BiliBiliPages.PagesData?> GetBiliBiliPagesDataAsync(string bvid, int p = 1)
  {
    var a = await GetBiliBiliPageAsync(bvid).ConfigureAwait(false);
    return GetBiliBiliPagesDataAsync(a, p);
  }

  /// <summary>
  ///   获取分P详细信息
  /// </summary>
  /// <param name="pages"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  private BiliBiliPages.PagesData? GetBiliBiliPagesDataAsync(BiliBiliPages? pages, int p)
  {
    if (pages?.Data is { Length: > 0 }) return pages.Code == 0 ? pages.Data.FirstOrDefault(e => e.Page == p) : null;
    return null;
  }
}