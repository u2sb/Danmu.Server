using System;
using System.ComponentModel.DataAnnotations;

namespace Danmaku.Model
{
    public class DanmakuModel : DanmakuGet
    {
        [MaxLength(32)]
        public string Id { get; set; }
        [MaxLength(16)]
        public string Ip { get; set; }
        public string Referer { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class DanmakuGet
    {
        [MaxLength(8)]
        public float Time { get; set; }
        [MaxLength(1)]
        public int Type { get; set; }
        [MaxLength(10)]
        public int Color { get; set; }
        [MaxLength(16)]
        public string Author { get; set; }
        [MaxLength(255)]
        public string Text { get; set; }
    }
}