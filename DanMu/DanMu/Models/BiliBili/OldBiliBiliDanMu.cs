using System.ComponentModel;
using System.Xml.Serialization;
using DanMu.Models.Protos.BiliBili.Dm;
using Google.Protobuf.Collections;

namespace DanMu.Models.BiliBili;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot("i", Namespace = "", IsNullable = false)]
public class OldBiliBiliDanMu
{
  [XmlElement("chatserver")] public string ChatServer { get; set; } = "chat.bilibili.com";
  [XmlElement("chatid")] public int ChatId { get; set; }
  [XmlElement("mission")] public int Mission { get; set; }
  [XmlElement("maxlimit")] public int MaxLimit { get; set; } = 1500;
  [XmlElement("state")] public int State { get; set; }
  [XmlElement("real_name")] public string RealName { get; set; } = "0";
  [XmlElement("source")] public string Source { get; set; } = "e-r";
  [XmlElement("d")] public D[]? D { get; set; }

  public static explicit operator OldBiliBiliDanMu(RepeatedField<DanmakuElem>? data)
  {
    var d = data?.Select(s => new D
    {
      P =
        $"{s.Progress / 1000f},{s.Mode},{s.Fontsize},{s.Color},{s.Ctime},{s.Pool},{s.MidHash},{s.Id}, {s.Weight}",
      Value = s.Content
    }).ToArray();
    return new OldBiliBiliDanMu { D = d, MaxLimit = data?.Count ?? 1500 };
  }
}

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
public class D
{
  [XmlAttribute("p")] public string? P { get; set; }

  [XmlText] public string? Value { get; set; }
}