using DanMu.Models.BiliBili;
using DanMu.Models.Protos.BiliBili.Dm;
using DanMu.Models.Settings;
using DanMu.Utils.Caching;
using Google.Protobuf;
using RestSharp;
using static WebApiProtobufFormatter.Utils.Serialize;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp(AppSettings setting, RestClient restClient, BiliBiliCaching caching)
{
  // 接口
  private const string BaseUrl = "https://api.bilibili.com";
  private const string PageUrl = "/x/player/pagelist";
  private const string DanMuUrl = "/x/v2/dm/list/seg.so";

  private readonly BiliBiliSetting _setting = setting.BiliBiliSetting;


  /// <summary>
  ///   获取B站弹幕 并返回通用弹幕格式
  /// </summary>
  /// <param name="id"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async ValueTask<DanmakuElem[]?> GetGenericDanMuAsync(string id = "", int p = 1)
  {
    var a = await GetDanMuAsync(id, p).ConfigureAwait(false);

    if (a != null) return a.Elems.ToArray();

    return Array.Empty<DanmakuElem>();
  }


  /// <summary>
  ///   获取B站弹幕
  /// </summary>
  /// <param name="id"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async ValueTask<DmSegMobileReply?> GetDanMuAsync(string id = "", int p = 1)
  {
    var a = await GetDanMuStreamAsync(id, p).ConfigureAwait(false);
    a.Position = 0;
    return Parse(a, DmSegMobileReply.Parser);
  }


  /// <summary>
  ///   获取B站弹幕数据流
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async ValueTask<Stream> GetDanMuStreamAsync(string bvid = "", int p = 1)
  {
    var page = await GetBiliBiliPagesDataAsync(bvid, p).ConfigureAwait(false);

    if (page != null && page.Cid != 0)
    {
      var a = await caching.DmGetOrSetAsync(page.Cid,
        async () =>
        {
          var dm = await GetDanMuNoCacheAsync(page).ConfigureAwait(false);
          var ms = new MemoryStream();
          dm.WriteTo(ms);
          ms.Position = 0;
          return ms;
        },
        TimeSpan.FromHours(_setting.DanMuCacheTime)).ConfigureAwait(false);

      if (a != null)
      {
        a.Position = 0;
        return a;
      }
    }

    return Stream.Null;
  }


  /// <summary>
  ///   获取B站弹幕无缓存
  /// </summary>
  /// <param name="page"></param>
  /// <returns></returns>
  private async ValueTask<DmSegMobileReply?> GetDanMuNoCacheAsync(BiliBiliPages.PagesData page)
  {
    var d = page.Duration / 360 + 1;
    var getDanMuTaskList = new List<Task<Stream?>>();

    for (var i = 0; i < d; i++)
    {
      var a = GetBiliBiliDataRawAsync(DanMuUrl, new Dictionary<string, string>
      {
        { "type", "1" /* 1 视频  2 漫画*/ },
        { "oid", page.Cid.ToString() },
        { "segment_index", (i + 1).ToString() }
      });

      getDanMuTaskList.Add(a.AsTask());
    }

    var danMuRawList = await Task.WhenAll(getDanMuTaskList).ConfigureAwait(false);

    var danMuSegList = danMuRawList.Where(w => w != null && w != Stream.Null)
      .Select(s => Parse(s!, DmSegMobileReply.Parser));

    var dmSeg = new DmSegMobileReply
    {
      State = 0
    };

    foreach (var danMuSeg in danMuSegList) dmSeg.Elems.Add(danMuSeg.Elems);

    if (dmSeg.Elems.Count > 0) return dmSeg;

    return null;
  }
}