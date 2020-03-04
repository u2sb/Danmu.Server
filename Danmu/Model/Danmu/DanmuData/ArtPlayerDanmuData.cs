using System;
using System.Net;

namespace Danmu.Model.Danmu.DanmuData
{
    public class ArtPlayerDanmuData : IDanmuData
    {
        /// <summary>
        ///     弹幕文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     弹幕时间
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        ///     弹幕颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///     弹幕字号
        /// </summary>
        public int Size { get; set; } = 25;

        /// <summary>
        ///     是否有边框
        /// </summary>
        public bool Border { get; set; } = false;

        /// <summary>
        ///     弹幕模式 0-滚动 1-固定
        /// </summary>
        public int Mode { get; set; }

        public BaseDanmuData ToBaseDanmuData()
        {
            return new BaseDanmuData
            {
                Time = Time,
                Mode = Mode,
                Color = Convert.ToInt32(Color.Replace("#", "0x"), 16),
                Size = Size,
                Text = Text
            };
        }

        public static explicit operator ArtPlayerDanmuData(BaseDanmuData data)
        {
            var t = data.Mode;
            if (t > 7) return null;
            return new ArtPlayerDanmuData
            {
                Time = data.Time,
                Mode = t == 4 || t == 5 ? 1 : 0,
                Color = $"#{data.Color:X}",
                Size = data.Size,
                Text = data.Text
            };
        }
    }

    public class ArtPlayerDanmuDataIn : ArtPlayerDanmuData
    {
        /// <summary>
        ///     视频的Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     评论者ip
        /// </summary>
        public IPAddress Ip { get; set; }

        /// <summary>
        ///     来源
        /// </summary>
        public string Referer { get; set; }
    }
}
