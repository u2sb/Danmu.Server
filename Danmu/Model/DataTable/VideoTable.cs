using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Danmu.Model.DataTable
{
    [Table("Video")]
    public class VideoTable
    {
        /// <summary>
        ///     主键 自增ID
        /// </summary>
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///     视频VID
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string Vid { get; set; }

        [Column(TypeName = "jsonb")] public Referer Referer { get; set; }

        /// <summary>
        ///     生成时间 UTC
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     修改时间 UTC
        /// </summary>
        public DateTime UpDateTime { get; set; } = DateTime.UtcNow;
    }

    public class Referer
    {
        public Referer() { }

        public Referer(string u)
        {
            var uri = new Uri(u);
            Protocol = uri.Scheme;
            Host = uri.Host;
            Port = uri.Port;
            Path = uri.AbsolutePath;
            Fragment = uri.Fragment;
        }

        public Referer(Uri uri)
        {
            Protocol = uri.Scheme;
            Host = uri.Host;
            Port = uri.Port;
            Path = uri.AbsolutePath;
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
        ///     参数
        /// </summary>
        public string Fragment { get; set; }

        public Uri ToUri()
        {
            return new Uri(
                    $"{Protocol}://{Host}:{Port}{Path}{(string.IsNullOrEmpty(Query) ? null : $"?{Query}")}{(string.IsNullOrEmpty(Fragment) ? null : $"?{Fragment}")}");
        }
    }
}