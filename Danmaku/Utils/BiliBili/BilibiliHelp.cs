using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Danmaku.Model.Danmaku;
using Danmaku.Utils.AppConfiguration;

namespace Danmaku.Utils.BiliBili
{
    public class BiliBiliHelp
    {
        private readonly HttpClient _deflateClient;
        private readonly HttpClient _gzipClient;

        public BiliBiliHelp(IAppConfiguration appConfiguration, IHttpClientFactory httpClientFactory)
        {
            _deflateClient = httpClientFactory.CreateClient("deflate");
            _gzipClient = httpClientFactory.CreateClient("gzip");
            var settings = appConfiguration.GetAppSetting();
            if (!string.IsNullOrEmpty(settings.BCookie))
                _deflateClient.DefaultRequestHeaders.Add("Cookie", settings.BCookie);
        }

        /// <summary>
        ///     获取视频Cid和分P信息
        /// </summary>
        /// <param name="aid">视频的aid</param>
        /// <returns>Page信息</returns>
        public async Task<List<BilibiliPage>> GetBilibiliPage(string aid)
        {
            var result = _gzipClient.GetStreamAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}");
            var pages = await JsonSerializer.DeserializeAsync<List<BilibiliPage>>(await result);
            return pages;
        }

        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="p">分p</param>
        /// <returns>cid</returns>
        public int GetCid(List<BilibiliPage> pages, int p)
        {
            var cid = 0;
            foreach (var page in pages)
                if (page.Page == p)
                    cid = page.Cid;
            return cid;
        }

        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="aid">视频的aid</param>
        /// <param name="p">分p</param>
        /// <returns>cid</returns>
        public async Task<int> GetCid(string aid, int p)
        {
            var pages = await GetBilibiliPage(aid);
            return GetCid(pages, p);
        }

        /// <summary>
        ///     获取弹幕列表
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <returns>弹幕列表</returns>
        public async Task<List<DanmakuData>> GetBiDanmaku(string cid)
        {
            return (await GetBiDanmakuDataAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}")).ToList();
        }

        /// <summary>
        ///     获取历史弹幕列表
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <param name="date">历史日期</param>
        /// <returns>弹幕列表</returns>
        public Task<List<DanmakuData>> GetBiDanmaku(string cid, string[] date)
        {
            return Task.Run(() => date.Select(async s => await GetBiDanmakuDataAsync(
                                               $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}"))
                                      .SelectMany(s => s.Result).ToList());
        }

        /// <summary>
        ///     获取弹幕列表
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <returns>原始弹幕</returns>
        public Task<Stream> GetBiDanmakuRaw(string cid)
        {
            return GetBiDanmakuDataRawAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}");
        }

        private Task<Stream> GetBiDanmakuDataRawAsync(string url)
        {
            return _deflateClient.GetStreamAsync(url);
        }

        private async Task<IEnumerable<DanmakuData>> GetBiDanmakuDataAsync(string url)
        {
            var bd = new BilibiliDanmakuData(await GetBiDanmakuDataRawAsync(url));
            return bd.ToDanmakuDataList();
        }
    }
}