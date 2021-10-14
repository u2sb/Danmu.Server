using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LiteDB;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Danmu.Models.Danmus.DataTables
{
    [Table("Video")]
    public class VideoTable
    {
        /// <summary>
        ///     主键 自增ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     视频VID
        /// </summary>
        [Required, MaxLength(72)]
        public string Vid { get; set; }

        [Column(TypeName = "jsonb")] public Referer Referer { get; set; }

        /// <summary>
        ///     生成时间 UTC
        /// </summary>
        [Column(TypeName = "timestamp(3)")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     修改时间 UTC
        /// </summary>
        [Column(TypeName = "timestamp(3)")]
        public DateTime UpDateTime { get; set; } = DateTime.UtcNow;
    }

    public class Referer
    {
        public Referer() { }

        public Referer(string u) : this(new Uri(u)) { }

        public Referer(Uri uri)
        {
            Protocol = uri.Scheme;
            Host = uri.Host;
            Port = uri.Port;
            Path = uri.AbsolutePath;
            Query = uri.Query;
            Fragment = uri.Fragment;
        }

        /// <summary>
        ///     连接协议
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        ///     视频播放器所在网站
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     视频播放器网页路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     查询参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        ///     Fragment
        /// </summary>
        public string Fragment { get; set; }

        /// <summary>
        ///     json序列化
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        ///     json反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Referer FromJson(string json)
        {
            return JsonSerializer.Deserialize<Referer>(json);
        }

        public Uri ToUri()
        {
            return new Uri($"{Protocol}://{Host}:{Port}{Path}{Query}{Fragment}");
        }
    }
}
