using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Danmaku.Model;
using Danmaku.Utils.AppConfiguration;

namespace Danmaku.Utils.BiliBili
{
    public class BiliBiliHelp : IBiliBiliHelp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _settings;

        public BiliBiliHelp(IAppConfiguration appConfiguration, IHttpClientFactory httpClientFactory)
        {
            _settings = appConfiguration.GetAppSetting();
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<BilibiliPage>> GetBilibiliPage(string aid)
        {
            var client = _httpClientFactory.CreateClient("gzip");
            var result = await client.GetStringAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}");
            var pages = JsonSerializer.Deserialize<List<BilibiliPage>>(result);
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
            return (await GetBiDanmakuDataAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}", null)).ToList();
        }

        public async Task<List<DanmakuData>> GetBiDanmaku(string cid, string[] date)
        {
            return await Task.Run(() => date.Select(async s => await GetBiDanmakuDataAsync(
                    $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}",
                    _settings.BCookie)).SelectMany(s => s.Result).ToList());
        }

        public async Task<string> GetBiDanmakuRaw(string cid)
        {
            return await GetBiDanmakuDataRawAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}", null);
        }



        private async Task<string> GetBiDanmakuDataRawAsync(string url, string cookie)
        {
            var client = _httpClientFactory.CreateClient("deflate");
            if (!string.IsNullOrEmpty(cookie)) client.DefaultRequestHeaders.Add("Cookie", cookie);
            return await client.GetStringAsync(url);
        }

        private async Task<IEnumerable<DanmakuData>> GetBiDanmakuDataAsync(string url, string cookie)
        {
            var result = GetBiDanmakuDataRawAsync(url, cookie);
            using var sr = new StringReader(await result);
            var mySerializer = new XmlSerializer(typeof(BilibiliDanmakuData));
            var bd = (BilibiliDanmakuData) mySerializer.Deserialize(sr);
            return bd.ToDanmakuDataList();
        }
    }
}