using System.Text.Json.Serialization;
using DanMu.Models.BiliBili;
using MemoryPack;

namespace DanMu.Models.ArtPlayer;

[MemoryPackable]
[GenerateTypeScript]
public partial class ArtPlayerDm
{
  /// <summary>
  ///   弹幕文本
  /// </summary>
  [MemoryPackOrder(0)]
  [JsonPropertyName("text")]
  public string? Text { get; set; }

  /// <summary>
  ///   弹幕位置  0: 滚动，1: 顶部，2: 底部
  /// </summary>
  [MemoryPackOrder(1)]
  [JsonPropertyName("model")]
  public int Model { get; set; }

  /// <summary>
  ///   弹幕颜色
  /// </summary>
  [MemoryPackOrder(2)]
  [JsonPropertyName("color")]
  public string? Color { get; set; } = "#FFF";

  /// <summary>
  ///   出现时间 (单位: s)
  /// </summary>
  [MemoryPackOrder(3)]
  [JsonPropertyName("time")]
  public int Time { get; set; }

  /// <summary>
  ///   是否描边
  /// </summary>
  [MemoryPackOrder(4)]
  [JsonPropertyName("border")]
  public bool Border { get; set; }

  /// <summary>
  ///   样式
  /// </summary>
  [MemoryPackOrder(5)]
  [JsonPropertyName("style")]
  public Dictionary<string, string>? Style { get; set; }


  /// <summary>
  ///   从bilibili弹幕转换
  /// </summary>
  /// <returns></returns>
  public static List<ArtPlayerDm> FromBilibiliDanMu(List<DanmakuElem>? elems)
  {
    return elems?.Select(s => new ArtPlayerDm
    {
      Text = s.Content,
      Model = s.Mode switch
      {
        4 => 2,
        5 => 1,
        _ => 0
      },
      Color = $"#{s.Color:X}",
      Time = s.Progress / 1000,
      Style = new Dictionary<string, string>
      {
        { "font-size", $"{s.FontSize}px" }
      }
    }).ToList() ?? new List<ArtPlayerDm>();
  }
}