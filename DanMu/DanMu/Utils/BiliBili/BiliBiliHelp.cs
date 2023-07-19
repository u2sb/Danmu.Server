using DanMu.Models.BiliBili;
using DanMu.Models.Protos.BiliBili.Dm;
using DanMu.Models.Settings;
using DanMu.Utils.Caching;
using Google.Protobuf;
using RestSharp;
using static WebApiProtobufFormatter.Utils.Serialize;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp
{
  // 接口
  private const string BaseUrl = "https://api.bilibili.com";
  private const string PageUrl = "/x/player/pagelist";
  private const string DanMuUrl = "/x/v2/dm/list/seg.so";
  private readonly BiliBiliCaching _caching;

  private readonly RestClient _restClient;
  private readonly BiliBiliSetting _setting;


  public BiliBiliHelp(AppSettings setting, RestClient restClient, BiliBiliCaching caching)
  {
    _restClient = restClient;
    _caching = caching;
    _setting = setting.BiliBiliSetting;
  }


  /// <summary>
  ///   获取B站弹幕 并返回通用弹幕格式
  /// </summary>
  /// <param name="id"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async Task<DanmakuElem[]?> GetGenericDanMuAsync(string id = "", int p = 1)
  {
    var a = await GetDanMuAsync(id, p);

    if (a != null) return a.Elems.ToArray();

    return Array.Empty<DanmakuElem>();
  }


  /// <summary>
  ///   获取B站弹幕
  /// </summary>
  /// <param name="id"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async Task<DmSegMobileReply?> GetDanMuAsync(string id = "", int p = 1)
  {
    var a = await GetDanMuStreamAsync(id, p);
    a.Position = 0;
    return Parse(a, DmSegMobileReply.Parser);
  }


  /// <summary>
  ///   获取B站弹幕数据流
  /// </summary>
  /// <param name="bvid"></param>
  /// <param name="p"></param>
  /// <returns></returns>
  public async Task<Stream> GetDanMuStreamAsync(string bvid = "", int p = 1)
  {
    var page = await GetBiliBiliPagesDataAsync(bvid, p);

    if (page != null && page.Cid != 0)
    {
      var a = await _caching.DmGetOrSetAsync(page.Cid,
        async () =>
        {
          var dm = await GetDanMuNoCacheAsync(page);
          var ms = new MemoryStream();
          dm.WriteTo(ms);
          ms.Position = 0;
          return ms;
        },
        TimeSpan.FromHours(_setting.DanMuCacheTime));

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
  private async Task<DmSegMobileReply?> GetDanMuNoCacheAsync(BiliBiliPages.PagesData page)
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

      getDanMuTaskList.Add(a);
    }

    var danMuRawList = await Task.WhenAll(getDanMuTaskList);

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