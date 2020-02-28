using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Danmaku.Model.Danmaku
{
    public class DanmakuData
    {
        public float Time { get; set; }

        public int Type { get; set; }

        public int Color { get; set; }

        [MaxLength(16)] public string Author { get; set; }

        [MaxLength(255)] public string Text { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static DanmakuData FromJson(string json)
        {
            return JsonSerializer.Deserialize<DanmakuData>(json);
        }
    }

    public class DanmakuDataInsert : DanmakuData
    {
        [MaxLength(36)] public string Id { get; set; }

        public IPAddress Ip { get; set; }
        public string Referer { get; set; }
    }
}