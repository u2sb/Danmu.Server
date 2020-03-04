using System;
using System.Net;
using System.Text.Json;

namespace Danmu.Model.Danmu.DanmuData
{
    public class BaseDanmuData : IDanmuData
    {
        /// <summary>
        ///     弹幕出现时间
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        ///     弹幕模式
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        ///     弹幕大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        ///     弹幕颜色
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        ///     时间戳
        /// </summary>
        public long TimeStamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        /// <summary>
        ///     弹幕池ID
        /// </summary>
        public int Pool { get; set; } = 0;

        /// <summary>
        ///     弹幕作者 名称
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        ///     弹幕作者ID
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        ///     弹幕内容
        /// </summary>
        public string Text { get; set; } = "";

        public BaseDanmuData ToBaseDanmuData()
        {
            return this;
        }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <returns>json字符串</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        ///     显示转换
        /// </summary>
        /// <param name="json">json数据</param>
        public static explicit operator BaseDanmuData(string json)
        {
            return JsonSerializer.Deserialize<BaseDanmuData>(json);
        }
    }

    public class BaseDanmuDataIn : BaseDanmuData
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
