using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Danmaku.Model;
using Danmaku.Utils.AppConfiguration;

namespace Danmaku.Utils.BiliBili
{
    public class BiliBiliHelp : IBiliBiliHelp
    {
        private readonly AppSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

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
            return await GetBiDanmakuDataAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}", null);
        }

        public async Task<List<DanmakuData>> GetBiDanmaku(string cid, string[] date)
        {
            var dgss = new List<List<DanmakuData>>();
            foreach (var da in date)
            {
                var b = await GetBiDanmakuDataAsync(
                    $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={da}",
                    _settings.BCookie);
                dgss.Add(b);
            }

            var dgs = new List<DanmakuData>();
            foreach (var dg in dgss) dgs = dgs.Concat(dg).ToList();
            return dgs;
        }

        private async Task<List<DanmakuData>> GetBiDanmakuDataAsync(string url, string cookie)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("deflate");
                if (!string.IsNullOrEmpty(cookie)) client.DefaultRequestHeaders.Add("Cookie", cookie);
                var result = await client.GetStringAsync(url);

                var xml = new XmlDocument();
                xml.LoadXml(result);
                var ds = xml.GetElementsByTagName("d");

                var dgs = new List<DanmakuData>();
                if (ds.Count == 0) return dgs;

                foreach (XmlNode d in ds)
                {
                    var ps = d.Attributes["p"].InnerText.Split(",");
                    var dg = new DanmakuData
                    {
                        Time = float.Parse(ps[0]),
                        Color = int.Parse(ps[3]),
                        Type = int.Parse(ps[5]),
                        Author = "",
                        Text = d.InnerText
                    };
                    dgs.Add(dg);
                }

                return dgs;
            }
            catch
            {
                return new List<DanmakuData>();
            }
        }
    }
}