using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Danmu.Models.Danmus.Danmu;
using LiteDB;

namespace Danmu.Models.Danmus.DataTables
{
    [Table("Danmu")]
    public class DanmuTable
    {
        /// <summary>
        ///     Id
        /// </summary>
        [Key]
        public Guid Id { get; set; } = new Guid();

        /// <summary>
        ///     弹幕所在视频
        /// </summary>
        [ Required, MaxLength(72)]
        public string Vid { get; set; }

        /// <summary>
        ///     弹幕数据
        /// </summary>
        [Required]
        public BaseDanmuData Data { get; set; }

        /// <summary>
        ///     弹幕发送者IP
        /// </summary>
        public IPAddress Ip { get; set; }

        /// <summary>
        ///     是否被删除
        /// </summary>
        [Required]
        public bool IsDelete { get; set; } = false;

        /// <summary>
        ///     关联到video表
        /// </summary>
        public VideoTable Video { get; set; }

        /// <summary>
        ///     生成时间 UTC
        /// </summary>
        [Column(TypeName = "timestamp(0)")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     修改时间 UTC
        /// </summary>
        [Column(TypeName = "timestamp(0)")]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
    }
}
