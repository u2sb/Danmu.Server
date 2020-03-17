using System;
using System.IO;
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
        /// <param name="aid">视频的aid</param>
        /// <returns></returns>
        private async Task<byte[]> GetBiliBiliPageRawAsync(int aid)
        {
            return await _cache.GetOrCreateCache($"aid{aid}", TimeSpan.FromMinutes(_setting.CidCacheTime),
                    async s =>
                    {
                        var gzipClient = _httpClientFactory.CreateClient(Gzip);
                        var response =
                                await gzipClient.GetAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}");
                        if (response.IsSuccessStatusCode) return await response.Content.ReadAsByteArrayAsync();
                        return new byte[0];
                    });
        }

        /// <summary>
        ///     获取BiliBili弹幕原始数据
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        private async Task<Stream> GetDanmuRawAsync(string url)
        {
            var key = Md5.GetMd5(url);
            var result = await _cache.GetOrCreateCache(key, TimeSpan.FromMinutes(_setting.CidCacheTime), async s =>
            {
                var response = await _deflateClient.GetAsync(url);
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsByteArrayAsync();
                return new byte[0];
            });

            return result.Length == 0 ? Stream.Null : new MemoryStream(result);
        }

        public Task<Stream> GetDanmuRawByCidTask(int cid)
        {
            return GetDanmuRawAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}");
        }
    }
}
