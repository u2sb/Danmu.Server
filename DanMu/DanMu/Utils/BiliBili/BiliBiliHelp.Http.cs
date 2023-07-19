using RestSharp;

namespace DanMu.Utils.BiliBili;

public partial class BiliBiliHelp
{
  /// <summary>
  ///   请求原始数据
  /// </summary>
  /// <param name="path"></param>
  /// <param name="queryParams"></param>
  /// <returns></returns>
  private async Task<Stream?> GetBiliBiliDataRawAsync(string path, Dictionary<string, string>? queryParams)
  {
    var request = new RestRequest(BaseUrl + path);

    if (queryParams != null)
      foreach (var item in queryParams)
        request.AddQueryParameter(item.Key, item.Value, false);
    return await _restClient.DownloadStreamAsync(request);
  }
}