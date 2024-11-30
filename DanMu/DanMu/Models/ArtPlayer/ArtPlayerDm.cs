using System.Text.Json.Serialization;
using DanMu.Models.BiliBili;
using MessagePack;

namespace DanMu.Models.ArtPlayer;

[MessagePackObject]
public class ArtPlayerDm
{
  /// <summary>
  ///   弹幕文本
  /// </summary>
  [JsonPropertyName("text")]
  [Key(0)]
  public string? Text { get; set; }

  /// <summary>
  ///   弹幕位置  0: 滚动，1: 顶部，2: 底部
  /// </summary>
  [JsonPropertyName("model")]
  [Key(1)]
  public int Model { get; set; }

  /// <summary>
  ///   弹幕颜色
  /// </summary>
  [JsonPropertyName("color")]
  [Key(2)]
  public string? Color { get; set; } = "#FFF";

  /// <summary>
  ///   出现时间 (单位: s)
  /// </summary>
  [JsonPropertyName("time")]
  [Key(3)]
  public int Time { get; set; }

  /// <summary>
  ///   是否描边
  /// </summary>
  [JsonPropertyName("border")]
  [Key(4)]
  public bool Border { get; set; }

  /// <summary>
  ///   样式
  /// </summary>
  [JsonPropertyName("style")]
  [Key(5)]
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
    }).ToList() ?? [];
  }
}