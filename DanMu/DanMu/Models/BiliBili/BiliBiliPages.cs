using System.Text.Json.Serialization;

namespace DanMu.Models.BiliBili;

public class BiliBiliPages
{
  [JsonPropertyName("code")] public int Code { get; set; } = -1;
  [JsonPropertyName("data")] public PagesData[] Data { get; set; } = Array.Empty<PagesData>();


  public class PagesData
  {
    [JsonPropertyName("cid")] public int Cid { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("part")] public string? Part { get; set; }
    [JsonPropertyName("duration")] public int Duration { get; set; }
    [JsonPropertyName("dimension")] public Dimension? Dimension { get; set; }
  }

  public class Dimension
  {
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("rotate")] public int Rotate { get; set; }
  }
}