using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Xml;
using Danmaku.Model;
using Newtonsoft.Json;

namespace Danmaku.Utils.BiliBili
{
    public class BilibiliHelp : IBilibiliHelp
    {
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

        public List<DanmakuGet> GetBiDanmaku(string cid)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var gResult = client.DownloadDataTaskAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}")
                        .Result;
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
            catch
            {
                return new List<DanmakuGet>();
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