using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Danmaku.Model
{
    public class DanmakuModel : DanmakuGet
    {
        [MaxLength(32)] public string Id { get; set; }

        public long Ip { get; set; }
        public string Referer { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class DanmakuGet
    {
        public float Time { get; set; }

        public int Type { get; set; }

        public int Color { get; set; }

        [MaxLength(16)] public string Author { get; set; }

        [MaxLength(255)] public string Text { get; set; }
    }

    public class DanmakuInsert
    {
        public DanmakuInsert(DanmakuModel danmaku)
        {
            Id = danmaku.Id;
            Ip =  danmaku.Ip;
            Referer = danmaku.Referer;
            Date = danmaku.Date.Ticks;
            Data = new object[]
            {
                danmaku.Time,
                danmaku.Type,
                danmaku.Color,
                danmaku.Author,
                danmaku.Text
            };
        }
        public string Id { get; set; }

        public object[] Data { get; set; }

        public long Ip { get; set; }
        public string Referer { get; set; }
        public long Date { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}