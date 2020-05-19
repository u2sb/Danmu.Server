using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Utils.BiliBili
{
    public partial class BiliBiliHelp
    {
        /// <summary>
        ///     获取视频的Page原始数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<Stream> GetBiliBiliPageRawAsync(string url)
        {
            var client = _httpClientFactory.CreateClient(Deflate);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStreamAsync();
            return Stream.Null;
        }

        /// <summary>
        ///     获取BiliBili弹幕原始数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="useCookie"></param>
        /// <returns></returns>
        private async Task<Stream> GetDanmuRawAsync(string url, bool useCookie = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (useCookie && !string.IsNullOrEmpty(_setting.Cookie))
                request.Headers.Add("Cookie", _setting.Cookie);
            var client = _httpClientFactory.CreateClient(Deflate);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStreamAsync();
            return Stream.Null;
        }
    }
}
