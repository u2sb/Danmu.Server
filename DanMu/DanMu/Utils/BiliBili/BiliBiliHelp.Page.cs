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
  private async Task<BiliBiliPages?> GetBiliBiliPageAsync(string bvid)
  {
    //使用缓存
    return await _caching.PagesGetOrSetAsync(bvid, async () =>
    {
      var a = await GetBiliBiliDataRawAsync(PageUrl, new Dictionary<string, string> { { "bvid", bvid } });
      if (a != null)
        return await JsonSerializer.DeserializeAsync<BiliBiliPages>(a);
      return null;
    }, TimeSpan.FromHours(_setting.PageCacheTime));
  }

  /// <summary>
  ///   获取分P详细信息
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async Task<BiliBiliPages.PagesData?> GetBiliBiliPagesDataAsync(string bvid, int p = 1)
  {
    var a = await GetBiliBiliPageAsync(bvid);
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