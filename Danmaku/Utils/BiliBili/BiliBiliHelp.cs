using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Danmaku.Model;

namespace Danmaku.Utils.BiliBili
{
    public class BiliBiliHelp : IBiliBiliHelp
    {
        public List<BilibiliPage> GetBilibiliPage(string aid)
        {
            var handler = new SocketsHttpHandler {AutomaticDecompression = DecompressionMethods.GZip};
            using var client = new HttpClient(handler);
            var result = client.GetStringAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}").Result;
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

        public int GetCid(string aid, int p)
        {
            var pages = GetBilibiliPage(aid);
            return GetCid(pages, p);
        }


        public List<DanmakuData> GetBiDanmaku(string cid)
        {
            return GetBiDanmakuDataAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}", null).Result;
        }

        public List<DanmakuData> GetBiDanmaku(string cid, string[] date)
        {
            var dgss = new List<Task<List<DanmakuData>>>();
            foreach (var da in date)
            {
                var b = GetBiDanmakuDataAsync(
                    $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={da}",
                    AppConfiguration.Config.BCookie);
                dgss.Add(b);
            }

            var dgs = new List<DanmakuData>();
            foreach (var dg in dgss) dgs = dgs.Concat(dg.Result).ToList();
            return dgs;
        }

        private async Task<List<DanmakuData>> GetBiDanmakuDataAsync(string url, string cookie)
        {
            var handler = new SocketsHttpHandler {AutomaticDecompression = DecompressionMethods.Deflate};
            using var client = new HttpClient(handler);
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
    }
}