using System.Text.Json.Serialization;

namespace Danmu.Model.Danmu.BiliBili
{
    public class BiliBiliPage
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public DataObj[] Data { get; set; }

        public class DataObj
        {
            /// <summary>
            ///     分P
            /// </summary>
            [JsonPropertyName("page")]
            public int Page { get; set; }

            /// <summary>
            ///     标题
            /// </summary>
            [JsonPropertyName("part")]
            public string Part { get; set; }

            /// <summary>
            ///     cid
            /// </summary>
            [JsonPropertyName("cid")]
            public int Cid { get; set; }
        }
    }
}
