using System.Text.Json.Serialization;

namespace Danmu.Model.Danmu.BiliBili
{
    public class BvidInfo
    {
        [JsonPropertyName("code")] public int Code { get; set; } = 1;

        [JsonPropertyName("data")] public DataObj Data { get; set; }

        public class DataObj
        {
            [JsonPropertyName("aid")] public int Aid { get; set; }

            [JsonPropertyName("bvid")] public string Bvid { get; set; }
        }
    }
}
