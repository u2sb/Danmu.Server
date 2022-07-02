using System.ComponentModel;
using System.Xml.Serialization;
using DanMu.Models.DanMu.Generic.V1;

namespace DanMu.Models.DanMu.BiliBili.V1;

public class BiliBiliDanMu
{
    [XmlElement("d")] public D[]? D { get; set; }

    public static explicit operator BiliBiliDanMu(GenericDanMu[]? data)
    {
        var d = data?.Select(s => new D
        {
            P =
                $"{s.Time / 1000f},{s.Mode},{s.FontSize},{s.Color},{s.CTime},{s.Pool},{s.Author},{s.Id}, 0",
            Value = s.Content
        }).ToArray();
        return new BiliBiliDanMu { D = d };
    }

    public static explicit operator BiliBiliDanMu(List<GenericDanMu>? data)
    {
        var d = data?.Select(s => new D
        {
            P =
                $"{s.Time / 1000f},{s.Mode},{s.FontSize},{s.Color},{s.CTime},{s.Pool},{s.Author},{s.Id}, 0",
            Value = s.Content
        }).ToArray();
        return new BiliBiliDanMu { D = d };
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