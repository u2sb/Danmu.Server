using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Danmaku.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Danmaku.Utils.BiliBili
{
    public class BilibiliHelp : IBilibiliHelp
    {
        private readonly AppConfiguration _appConfiguration;

        public BilibiliHelp(IConfiguration configuration)
        {
            _appConfiguration = new AppConfiguration(configuration);
        }

        /// <summary>
        ///     获得cid
        /// </summary>
        /// <param name="aid">aid</param>
        /// <param name="p">p</param>
        /// <returns>cid</returns>
        public int GetCid(string aid, int p)
        {
            var cid = 0;
            try
            {
                using (var client = new WebClient())
                {
                    var gResult = client.DownloadDataTaskAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}")
                        .Result;
                    var result = DeGzip(gResult);
                    var pages = JsonConvert.DeserializeObject<List<BilibiliPage>>(result);
                    foreach (var page in pages)
                        if (page.Page == p)
                            cid = page.Cid;
                }

                return cid;
            }
            catch
            {
                return cid;
            }
        }

        /// <summary>
        ///     使用cid获得弹幕列表
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<DanmakuGet> GetBiDanmaku(string cid)
        {
            return GetBiDanmakuDataAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}").Result;
        }

        /// <summary>
        ///     使用cid获得历史弹幕
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<DanmakuGet> GetBiDanmaku(string cid, string[] date)
        {
            var dgss = new List<Task<List<DanmakuGet>>>();
            foreach (var da in date)
            {
                var b = GetBiDanmakuDataAsync(
                    $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={da}");
                dgss.Add(b);
            }
            var dgs = new List<DanmakuGet>();
            foreach (var dg in dgss) dgs = dgs.Concat(dg.Result).ToList();

            return dgs;
        }

        /// <summary>
        ///     获取弹幕
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<List<DanmakuGet>> GetBiDanmakuDataAsync(string url)
        {
            using (var client = new WebClient())
            {
                if (!string.IsNullOrWhiteSpace(_appConfiguration.BCookie))
                    client.Headers.Set("Cookie", _appConfiguration.BCookie);
                var gResult = await client.DownloadDataTaskAsync(url);
                var result = DeDeflate(gResult);
                var xml = new XmlDocument();
                xml.LoadXml(result);
                var ds = xml.GetElementsByTagName("d");
                var dgs = new List<DanmakuGet>();
                foreach (XmlNode d in ds)
                {
                    var dg = new DanmakuGet();
                    var ps = d.Attributes["p"].InnerText.Split(",");
                    dg.Time = float.Parse(ps[0]);
                    dg.Color = int.Parse(ps[3]);
                    dg.Type = int.Parse(ps[5]);
                    dg.Author = "";
                    dg.Text = d.InnerText;
                    dgs.Add(dg);
                }

                return dgs;
            }
        }


        /// <summary>
        ///     gzip解压
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string DeGzip(byte[] bytes)
        {
            string s = null;
            using (var stream = new MemoryStream(bytes))
            {
                using (var gZipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gZipStream))
                    {
                        s = reader.ReadToEndAsync().Result;
                    }
                }
            }

            return s;
        }

        /// <summary>
        ///     deflate解压
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string DeDeflate(byte[] bytes)
        {
            string s = null;
            using (var stream = new MemoryStream(bytes))
            {
                using (var gZipStream = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gZipStream))
                    {
                        s = reader.ReadToEndAsync().Result;
                    }
                }
            }

            return s;
        }
    }
}