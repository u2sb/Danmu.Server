using System.ComponentModel;
using ProtoBuf;

namespace DanMu.Models.BiliBili;

[ProtoContract]
public class DmSegMobileReply : IExtensible
{
  private IExtension? _pbnExtensionData;

  /// <summary>
  ///   弹幕条目
  /// </summary>
  [ProtoMember(1, Name = "elems", IsRequired = true)]
  public DanmakuElem[] Elems { get; set; } = [];

  IExtension IExtensible.GetExtensionObject(bool createIfMissing)
  {
    return Extensible.GetExtensionObject(ref _pbnExtensionData, createIfMissing);
  }
}

[ProtoContract]
public class DanmakuElem : IExtensible
{
  private IExtension? _pbnExtensionData;

  /// <summary>
  ///   弹幕 Id
  /// </summary>
  [ProtoMember(1, Name = "id")]
  public long Id { get; set; }

  /// <summary>
  ///   弹幕出现时间 (单位ms)
  /// </summary>
  [ProtoMember(2, Name = "progress")]
  public int Progress { get; set; }

  /// <summary>
  ///   弹幕类型 1 2 3:普通弹幕 4:底部弹幕 5:顶部弹幕 6:逆向弹幕 7:高级弹幕 8:代码弹幕 9:BAS弹幕(pool必须为2)
  /// </summary>
  [ProtoMember(3, Name = "mode")]
  public int Mode { get; set; }

  /// <summary>
  ///   弹幕字号
  /// </summary>
  [ProtoMember(4, Name = "fontsize")]
  public int FontSize { get; set; }

  /// <summary>
  ///   弹幕颜色
  /// </summary>
  [ProtoMember(5, Name = "color")]
  public uint Color { get; set; }

  /// <summary>
  ///   发送者mid hash
  /// </summary>
  [ProtoMember(6)]
  [DefaultValue("")]
  public string MidHash { get; set; } = "";

  /// <summary>
  ///   弹幕正文
  /// </summary>
  [ProtoMember(7, Name = "content")]
  [DefaultValue("")]
  public string Content { get; set; } = "";

  /// <summary>
  ///   发送时间
  /// </summary>
  [ProtoMember(8, Name = "ctime")]
  public long CTime { get; set; }

  /// <summary>
  ///   权重 用于屏蔽等级 区间:[1,10]
  /// </summary>
  [ProtoMember(9, Name = "weight")]
  public int Weight { get; set; }

  /// <summary>
  ///   弹幕池 0:普通池 1:字幕池 2:特殊池(代码/BAS弹幕)
  /// </summary>
  [ProtoMember(11, Name = "pool")]
  public int Pool { get; set; }

  IExtension IExtensible.GetExtensionObject(bool createIfMissing)
  {
    return Extensible.GetExtensionObject(ref _pbnExtensionData, createIfMissing);
  }
}