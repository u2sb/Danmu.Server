using Flurl.Http;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp
{
  // 接口
  public const string BaseUrl = "https://api.bilibili.com";
  private const string PageUrl = "/x/player/pagelist";
  private const string DanMuUrl = "/x/v2/dm/list/seg.so";

  /// <summary>
  ///   请求原始数据
  /// </summary>
  /// <param name="path"></param>
  /// <param name="queryParams"></param>
  /// <param name="ct"></param>
  /// <returns></returns>
  private async ValueTask<Stream> GetBiliBiliDataRawAsync(string path, Dictionary<string, string>? queryParams,
    CancellationToken ct = default)
  {
    var request = _flurlClient.Request(path);

    if (queryParams is { Count: > 0 })
      foreach (var item in queryParams)
        request.SetQueryParam(item.Key, item.Value);

    if (!string.IsNullOrWhiteSpace(_setting.Cookie))
      request.WithHeader("Cookie", _setting.Cookie);
    
    return await request.GetStreamAsync(cancellationToken: ct);
  }
}