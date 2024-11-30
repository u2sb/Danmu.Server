using System.Text.Json.Serialization;

namespace DanMu.Models.BiliBili;

public class BiliBiliPages
{
  [JsonPropertyName("code")] public int Code { get; init; } = -1;
  [JsonPropertyName("data")] public PagesData[] Data { get; init; } = [];


  public class PagesData
  {
    [JsonPropertyName("cid")] public int Cid { get; init; }

    [JsonPropertyName("page")] public int Page { get; init; }

    // [JsonPropertyName("part")] public string? Part { get; init; }
    [JsonPropertyName("duration")] public int Duration { get; init; }
    // [JsonPropertyName("dimension")] public Dimension? Dimension { get; init; }
  }

  public class Dimension
  {
    [JsonPropertyName("width")] public int Width { get; init; }
    [JsonPropertyName("height")] public int Height { get; init; }
    [JsonPropertyName("rotate")] public int Rotate { get; init; }
  }
}