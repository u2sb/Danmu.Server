using System.ComponentModel;

namespace Danmaku.Model.Danmaku
{
    public class ArtPlayerDanmaku
    {
        public ArtPlayerDanmaku() { }

        public ArtPlayerDanmaku(DplayerDanmaku d)
        {
            Text = d.Text;
            Time = d.Time;
            Color = $"#{d.Color:X}";
            Mode = d.Type == 0 ? 0 : 1;
        }

        public string Text { get; set; }
        public float Time { get; set; }
        public string Color { get; set; }
        [DefaultValue(25)] public int Size { get; set; }
        [DefaultValue(false)] public bool Border { get; set; }
        [DefaultValue(0)] public int Mode { get; set; }
    }
}
