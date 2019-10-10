using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Danmaku.Model
{
	public class DanmakuWebResult
	{
		public DanmakuWebResult(){}

		public DanmakuWebResult(int code) => Code = code;

		public DanmakuWebResult(List<DanmakuData> data) =>
			Data = data.Select(s => new object[]
				{s.Time, s.Type, s.Color, HttpUtility.HtmlEncode(s.Author), HttpUtility.HtmlEncode(s.Text)}).ToList();
		
		/// <summary>
        ///     代码，0正常 1错误
        /// </summary>
        [DefaultValue(0)]
		[JsonPropertyName("code")]
		public int Code { get; set; }

        /// <summary>
        ///     数据
        /// </summary>
        [JsonPropertyName("data")]
		public List<object[]> Data { get; }

		public static implicit operator string(DanmakuWebResult result) => JsonSerializer.Serialize(result);
	}

	public class DanmakuData
	{
		public float Time { get; set; }

		public int Type { get; set; }

		public int Color { get; set; }

		[MaxLength(16)] public string Author { get; set; }

		[MaxLength(255)] public string Text { get; set; }

		public string ToJson() => JsonSerializer.Serialize(this);

		public static DanmakuData FromJson(string json) => JsonSerializer.Deserialize<DanmakuData>(json);
	}

	public class DanmakuDataInsert : DanmakuData
	{
		[MaxLength(36)] public string Id { get; set; }

		public IPAddress Ip { get; set; }
		public string Referer { get; set; }
	}
}