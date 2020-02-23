using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Danmaku.Model;
using Danmaku.Utils.AppConfiguration;

namespace Danmaku.Utils.BiliBili
{
    public class BiliBiliHelp : IBiliBiliHelp
    {
        private readonly AppSettings _settings;
        private readonly HttpClient _deflateClient;
        private readonly HttpClient _gzipClient;

        public BiliBiliHelp(IAppConfiguration appConfiguration, IHttpClientFactory httpClientFactory)
        {
            _settings = appConfiguration.GetAppSetting();
            _deflateClient = httpClientFactory.CreateClient("deflate");
            _gzipClient = httpClientFactory.CreateClient("gzip");
            if (!string.IsNullOrEmpty(_settings.BCookie)) _deflateClient.DefaultRequestHeaders.Add("Cookie", _settings.BCookie);
        }

        public async Task<List<BilibiliPage>> GetBilibiliPage(string aid)
        {
            var result = _gzipClient.GetStreamAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}");
            var pages = await JsonSerializer.DeserializeAsync<List<BilibiliPage>>(await result);
            return pages;
        }

        public int GetCid(List<BilibiliPage> pages, int p)
        {
            var cid = 0;
            foreach (var page in pages)
                if (page.Page == p)
                    cid = page.Cid;
            return cid;
        }

        public async Task<int> GetCid(string aid, int p)
        {
            var pages = await GetBilibiliPage(aid);
            return GetCid(pages, p);
        }

        public async Task<List<DanmakuData>> GetBiDanmaku(string cid)
        {
            return (await GetBiDanmakuDataAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}")).ToList();
        }

        public Task<List<DanmakuData>> GetBiDanmaku(string cid, string[] date)
        {
            return Task.Run(() => date.Select(async s => await GetBiDanmakuDataAsync(
                    $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}")).SelectMany(s => s.Result).ToList());
        }

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
