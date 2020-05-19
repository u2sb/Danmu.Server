using System.Text.Json.Serialization;
using MessagePack;

namespace Danmu.Model.Danmu.BiliBili
{
    [MessagePackObject]
    public class BiliBiliPage
    {
        [JsonPropertyName("code")] [Key(0)] public int Code { get; set; }

        [JsonPropertyName("data")] [Key(1)] public DataObj[] Data { get; set; }

        [MessagePackObject]
        public class DataObj
        {
            /// <summary>
            ///     分P
            /// </summary>
            [JsonPropertyName("page")]
            [Key(0)]
            public int Page { get; set; }

            /// <summary>
            ///     标题
            /// </summary>
            [JsonPropertyName("part")]
            [Key(1)]
            public string Part { get; set; }

            /// <summary>
            ///     cid
            /// </summary>
            [JsonPropertyName("cid")]
            [Key(2)]
            public int Cid { get; set; }

            /// <summary>
            ///     时长
            /// </summary>
            [JsonPropertyName("duration")]
            [Key(3)]
            public int Duration { get; set; }

            [JsonPropertyName("dimension")]
            [Key(4)]
            public DimensionObj Dimension { get; set; }

            [MessagePackObject]
            public class DimensionObj
            {
                [JsonPropertyName("width")] [Key(0)] public int Width { get; set; }

                [JsonPropertyName("height")] [Key(1)] public int Height { get; set; }

                [JsonPropertyName("rotate")] [Key(2)] public int Rotate { get; set; }
            }
        }
    }
}
