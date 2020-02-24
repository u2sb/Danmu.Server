using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Danmaku.Model.WebResult
{
    public class WebResult<T>
    {
        public WebResult() { }

        public WebResult(int code)
        {
            Code = code;
        }

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
        public T Data { get; set; }

        public static implicit operator string(WebResult<T> result)
        {
            return JsonSerializer.Serialize(result);
        }
    }

    public class WebResult : WebResult<dynamic>
    {
        public WebResult() { }

        public WebResult(int code) : base(code) { }
    }
}