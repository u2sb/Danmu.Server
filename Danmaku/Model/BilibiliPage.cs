using System.Text.Json.Serialization;

namespace Danmaku.Model
{
    public class BilibiliPage
    {
        /// <summary>
        /// 分P
        /// </summary>
        [JsonPropertyName("page")]public int Page { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonPropertyName("name")] public string Name { get; set; }

        /// <summary>
        /// cid
        /// </summary>
        [JsonPropertyName("cid")] public int Cid { get; set; }
    }
}