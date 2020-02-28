using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Danmaku.Model.Danmaku
{
    public class DplayerDanmaku
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

        public static DplayerDanmaku FromJson(string json)
        {
            return JsonSerializer.Deserialize<DplayerDanmaku>(json);
        }
    }

    public class DplayerDanmakuInsert : DplayerDanmaku
    {
        [MaxLength(36)] public string Id { get; set; }

        public IPAddress Ip { get; set; }
        public string Referer { get; set; }
    }
}
