using System;

namespace Danmaku.Model
{
    public class DanmakuModel
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public float Time { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public int Type { get; set; }
        public string Ip { get; set; }
        public string Referer { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class DanmakuGet
    {
        public float Time { get; set; }
        public int Type { get; set; }
        public int Color { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}