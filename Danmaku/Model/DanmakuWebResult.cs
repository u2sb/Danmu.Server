using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

namespace Danmaku.Model
{
    public class DanmakuWebResult
    {
        public DanmakuWebResult()
        {
        }

        public DanmakuWebResult(int code)
        {
            Code = code;
        }


        public DanmakuWebResult(List<DanmakuData> data)
        {
            Code = 0;
            Data = data.Select(s => new object[]
                {s.Time, s.Type, s.Color, HttpUtility.HtmlEncode(s.Author), HttpUtility.HtmlEncode(s.Text)}).ToList();
        }


        /// <summary>
        ///     代码，0正常 1错误
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        public List<object[]> Data { get; set; }
    }

    public class DanmakuData
    {
        public DanmakuData()
        {
        }

        public DanmakuData(string json)
        {
            var t = System.Text.Json.JsonSerializer.Deserialize<DanmakuData>(json);
            Time = t.Time;
            Type = t.Type;
            Color = t.Color;
            Author = t.Author;
            Text = t.Text;
        }

        public float Time { get; set; }

        public int Type { get; set; }

        public int Color { get; set; }

        [MaxLength(16)] public string Author { get; set; }

        [MaxLength(255)] public string Text { get; set; }

        
        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }

    public class DanmakuDataInsert : DanmakuData
    {
        [MaxLength(36)] public string Id { get; set; }

        public IPAddress Ip { get; set; }
        public string Referer { get; set; }
    }
}