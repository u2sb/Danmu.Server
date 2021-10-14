using System.Text.Json.Serialization;

namespace Danmu.Models.Danmus.BiliBili
{
    public class BiliBiliPage
    {
        [JsonPropertyName("code")] public int Code { get; set; }

        [JsonPropertyName("data")] public DataObj[] Data { get; set; }


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

            /// <summary>
            ///     时长
            /// </summary>
            [JsonPropertyName("duration")]
            public int Duration { get; set; }

            [JsonPropertyName("dimension")] public DimensionObj Dimension { get; set; }

            public class DimensionObj
            {
                [JsonPropertyName("width")] public int Width { get; set; }

                [JsonPropertyName("height")] public int Height { get; set; }

                [JsonPropertyName("rotate")] public int Rotate { get; set; }
            }
        }
    }
}
