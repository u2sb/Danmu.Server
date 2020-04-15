using System;
using System.Net.Http;
using System.Threading.Tasks;
using Danmu.Utils.Common;
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
        private async Task<byte[]> GetBiliBiliPageRawAsync(string url)
        {
            var key = Md5.GetMd5(url);
            return await _cache.GetOrCreateHttpCacheAsync(key, TimeSpan.FromMinutes(_setting.CidCacheTime), async () =>
            {
                var client = _httpClientFactory.CreateClient(Deflate);
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsByteArrayAsync();
                return new byte[0];
            });
        }

        /// <summary>
        ///     获取BiliBili弹幕原始数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="useCookie"></param>
        /// <returns></returns>
        private async Task<byte[]> GetDanmuRawAsync(string url, bool useCookie = false)
        {
            var key = Md5.GetMd5(url);
            return await _cache.GetOrCreateHttpCacheAsync(key, TimeSpan.FromMinutes(_setting.DanmuCacheTime), async () =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (useCookie && !string.IsNullOrEmpty(_setting.Cookie))
                    request.Headers.Add("Cookie", _setting.Cookie);
                var client = _httpClientFactory.CreateClient(Deflate);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsByteArrayAsync();
                return new byte[0];
            });
        }
    }
}
