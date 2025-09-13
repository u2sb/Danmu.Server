using DanMu.Models.BiliBili;
using DanMu.Models.Settings;
using DanMu.Utils.Caching;
using Flurl.Http;
using Flurl.Http.Configuration;
using ProtoBuf;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp(AppSettings setting, IFlurlClientCache flurlClientCache, BiliBiliCaching caching)
{
  private readonly IFlurlClient _flurlClient = flurlClientCache.Get(nameof(BiliBiliHelp));
  private readonly BiliBiliSetting _setting = setting.BiliBiliSetting;


  /// <summary>
  ///   获取B站弹幕 并返回通用弹幕格式
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  public async ValueTask<IEnumerable<DanmakuElem>> GetGenericDanMuAsync(string bvid, int p = 1, CancellationToken ct = default)
  {
    var a = await GetDanMuAsync(bvid, p, ct).ConfigureAwait(false);

    return a != null ? a.Elems : [];
  }


  /// <summary>
  ///   获取B站弹幕
  /// </summary>
  /// <param name="id"></param>
  /// <param name="p"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  public async ValueTask<DmSegMobileReply?> GetDanMuAsync(string id, int p = 1, CancellationToken ct = default)
  {
    return await GetDanMuStreamAsync(id, p, ct).ConfigureAwait(false);
  }


  /// <summary>
  ///   获取B站弹幕数据流
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  private async ValueTask<DmSegMobileReply?> GetDanMuStreamAsync(string bvid, int p = 1,
    CancellationToken ct = default)
  {
    var page = await GetBiliBiliPagesDataAsync(bvid, p, ct).ConfigureAwait(false);

    if (page is not { Cid: > 0 }) return null;

    var a = await caching.DmGetOrSetAsync(page.Cid,
      async ct1 => await GetDanMuNoCacheAsync(page, ct1).ConfigureAwait(false),
      TimeSpan.FromHours(_setting.DanMuCacheTime), ct).ConfigureAwait(false);

    return a;
  }


  /// <summary>
  ///   获取B站弹幕无缓存
  /// </summary>
  /// <param name="page"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  private async ValueTask<DmSegMobileReply?> GetDanMuNoCacheAsync(BiliBiliPages.PagesData page,
    CancellationToken ct = default)
  {
    var d = page.Duration / 360 + 1;
    var getDanMuTaskList = new List<Task<Stream>>();

    for (var i = 0; i < d; i++)
    {
      var a = GetBiliBiliDataRawAsync(DanMuUrl, new Dictionary<string, string>
      {
        { "type", "1" /* 1 视频  2 漫画*/ },
        { "oid", page.Cid.ToString() },
        { "segment_index", (i + 1).ToString() }
      }, ct);

      getDanMuTaskList.Add(a.AsTask());
    }

    var danMuRawList = await Task.WhenAll(getDanMuTaskList).ConfigureAwait(false);

    var danMuSegList = danMuRawList.Where(w => w != Stream.Null)
      .Select(Serializer.Deserialize<DmSegMobileReply>);

    var dmSeg = new DmSegMobileReply
    {
      Elems = danMuSegList.SelectMany(s => s.Elems).ToArray()
    };

    return dmSeg.Elems.Length > 0 ? dmSeg : null;
  }
}