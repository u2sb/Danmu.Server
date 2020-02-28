using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;
using Danmaku.Model.Danmaku.Converter;

namespace Danmaku.Model.Danmaku
{
    public class DanmakuDataBase
    {
        public DanmakuDataBase() { }

        public DanmakuDataBase(DplayerDanmakuInsert date)
        {
            if (date != null)
            {
                Ip = date.Ip;
                Vid = date.Id;
                Referer = date.Referer;
                DplayerDanmaku = new DplayerDanmaku
                {
                    Time = date.Time,
                    Type = date.Type,
                    Author = date.Author,
                    Color = date.Color,
                    Text = date.Text
                };
            }
        }

        [Key] public Guid Id { get; set; }

        [MaxLength(36)] [Required] public string Vid { get; set; }

        [Column("DanmakuData",TypeName = "jsonb")]
        [JsonPropertyName("danmakuData")]
        [Required]
        public DplayerDanmaku DplayerDanmaku { get; set; }

        [JsonConverter(typeof(IPAddressConverter))]
        public IPAddress Ip { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Referer { get; set; }

        public bool IsDelete { get; set; } = false;
    }
}
