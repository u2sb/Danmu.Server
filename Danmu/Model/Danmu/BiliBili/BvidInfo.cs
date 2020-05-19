using System.Text.Json.Serialization;
using MessagePack;

namespace Danmu.Model.Danmu.BiliBili
{
    [MessagePackObject]
    public class BvidInfo
    {
        [JsonPropertyName("code")] [Key(0)] public int Code { get; set; } = 1;

        [JsonPropertyName("data")] [Key(1)] public DataObj Data { get; set; }

        [MessagePackObject]
        public class DataObj
        {
            [JsonPropertyName("aid")] [Key(0)] public int Aid { get; set; }

            [JsonPropertyName("bvid")] [Key(1)] public string Bvid { get; set; }
        }
    }
}
