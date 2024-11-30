using DanMu.Models.BiliBili;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp
{
  /// <summary>
  ///   获取视频详细
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  private async ValueTask<BiliBiliPages?> GetBiliBiliPageAsync(string bvid, CancellationToken ct = default)
  {
    //使用缓存
    return await caching.PagesGetOrSetAsync(bvid, async ct1 =>
      await GetBiliBiliDataRawAsync(PageUrl, new Dictionary<string, string> { { "bvid", bvid } }, ct1)
        .ConfigureAwait(false), TimeSpan.FromHours(_setting.PageCacheTime), ct);
  }

  /// <summary>
  ///   获取分P详细信息
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  private async ValueTask<BiliBiliPages.PagesData?> GetBiliBiliPagesDataAsync(string bvid, int p = 1,
    CancellationToken ct = default)
  {
    var a = await GetBiliBiliPageAsync(bvid, ct).ConfigureAwait(false);
    return GetBiliBiliPagesData(a, p);
  }

  /// <summary>
  ///   获取分P详细信息
  /// </summary>
  /// <param name="pages"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  private BiliBiliPages.PagesData? GetBiliBiliPagesData(BiliBiliPages? pages, int p)
  {
    if (pages?.Data is { Length: > 0 }) return pages.Code == 0 ? pages.Data.FirstOrDefault(e => e.Page == p) : null;
    return null;
  }
}