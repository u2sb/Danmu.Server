using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Danmaku.Model
{
    public class DanmakuDataBase
    {
        public DanmakuDataBase(){}

        public DanmakuDataBase(DanmakuDataInsert date)
        {
            Ip = date.Ip;
            Referer = date.Referer;
            Danmaku = new Danmaku
            {
                Id = date.Id,
                DanmakuData = new DanmakuData
                {
                    Time = date.Time,
                    Type = date.Type,
                    Author = date.Author,
                    Color = date.Color,
                    Text = date.Text
                }
            };
        }

        [Key] public Guid Id { get; set; }

        [Column(TypeName = "jsonb")] public Danmaku Danmaku { get; set; }

        public IPAddress Ip { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Referer { get; set; }
    }


    public class Danmaku
    {
        [MaxLength(36)]public string Id { get; set; }
        public DanmakuData DanmakuData { get; set; }
    }
}