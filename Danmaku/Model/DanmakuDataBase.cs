using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Danmaku.Model
{
	public class DanmakuDataBase
	{
		public DanmakuDataBase()
		{
		}

		public DanmakuDataBase(DanmakuDataInsert date)
		{
			if (date != null)
			{
				Ip = date.Ip;
				Vid = date.Id;
				Referer = date.Referer;
				DanmakuData = new DanmakuData
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

		[Column(TypeName = "jsonb")] public DanmakuData DanmakuData { get; set; }

		public IPAddress Ip { get; set; }

		public DateTime Date { get; set; } = DateTime.Now;

		public string Referer { get; set; }

		public bool IsDelete { get; set; } = false;
	}
}