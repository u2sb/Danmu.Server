using RestSharp;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp
{
  // 接口
  private const string BaseUrl = "https://api.bilibili.com";
  private const string PageUrl = "/x/player/pagelist";
  private const string DanMuUrl = "/x/v2/dm/list/seg.so";

  /// <summary>
  ///   请求原始数据
  /// </summary>
  /// <param name="path"></param>
  /// <param name="queryParams"></param>
  /// <returns></returns>
  private async ValueTask<Stream?> GetBiliBiliDataRawAsync(string path, Dictionary<string, string>? queryParams)
  {
    var request = new RestRequest(BaseUrl + path);

    if (queryParams is { Count: > 0 })
      foreach (var item in queryParams)
        request.AddQueryParameter(item.Key, item.Value, false);

    if (!string.IsNullOrWhiteSpace(_setting.Cookie))
      request.AddOrUpdateHeader("Cookie", _setting.Cookie);

    return await restClient.DownloadStreamAsync(request).ConfigureAwait(false);
  }
}