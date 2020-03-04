using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Danmu.Model.Danmu.DanmuData
{
    public class DplayerDanmuData : IDanmuData
    {
        /// <summary>
        ///     弹幕时间
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        ///     弹幕类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///     弹幕颜色
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        ///     弹幕作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     弹幕文字
        /// </summary>
        public string Text { get; set; }

        public BaseDanmuData ToBaseDanmuData()
        {
            var isId = int.TryParse(Author, out var authorId);
            return new BaseDanmuData
            {
                Time = Time,
                Mode = Type == 1 ? 5 : Type == 2 ? 4 : 1,
                Color = Color,
                AuthorId = isId ? authorId : 0,
                Author = isId ? "" : Author,
                Text = Text
            };
        }

        public static explicit operator DplayerDanmuData(BaseDanmuData data)
        {
            var t = data.Mode;
            if (t > 7) return null;
            return new DplayerDanmuData
            {
                Time = data.Time,
                Type = t == 4 ? 2 : t == 5 ? 1 : 0,
                Color = data.Color,
                Author = string.IsNullOrEmpty(data.Author) ? data.AuthorId.ToString() : data.Author,
                Text = data.Text
            };
        }
    }

    public class DplayerDanmuDataIn : DplayerDanmuData
    {
        /// <summary>
        ///     视频的id
        /// </summary>
        [MaxLength(36)]
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
