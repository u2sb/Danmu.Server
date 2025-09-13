using System.Text.Json.Serialization;
using DanMu.Models.BiliBili;
using MessagePack;

namespace DanMu.Models.DPlayer;

[MessagePackObject]
public class DPlayerDm
{
  /// <summary>
  ///   弹幕时间
  /// </summary>
  [Key(0)]
  public float Time { get; set; }

  /// <summary>
  ///   弹幕类型
  /// </summary>
  [Key(1)]
  public int Type { get; set; }

  /// <summary>
  ///   弹幕颜色
  /// </summary>
  [Key(2)]
  public uint Color { get; set; }

  /// <summary>
  ///   弹幕作者
  /// </summary>
  [Key(3)]
  public string Author { get; set; } = string.Empty;

  /// <summary>
  ///   弹幕文字
  /// </summary>
  [Key(4)]
  public string Text { get; set; } = string.Empty;

  /// <summary>
  ///   从bilibili弹幕转换
  /// </summary>
  /// <returns></returns>
  public static IEnumerable<DPlayerDm> FromBilibiliDanMu(IEnumerable<DanmakuElem>? elems)
  {
    return elems?.Select(s => new DPlayerDm
      {
        Text = s.Content,
        Type = s.Mode switch
        {
          4 => 2,
          5 => 1,
          _ => 0
        },
        Color = s.Color,
        Time = s.Progress / 1000f
      }
    ) ?? [];
  }
}

[MessagePackObject]
public class DPlayerDmResult
{
  [Key("code")]
  [JsonPropertyName("code")]
  public int Code { get; set; } = 0;

  [Key("data")]
  [JsonPropertyName("data")]
  [JsonConverter(typeof(DPlayerDmListJsonConverter))]
  public List<DPlayerDm> Data { get; set; } = [];
}