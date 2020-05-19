using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Danmu.Model.Danmu.DanmuData;
using MessagePack;

namespace Danmu.Model.Danmu.BiliBili
{
    [Serializable]
    [MessagePackObject]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("i", Namespace = "", IsNullable = false)]
    public class DanmuDataBiliBili
    {
        public DanmuDataBiliBili() { }

        public DanmuDataBiliBili(Stream s)
        {
            var serializer = new XmlSerializer(typeof(DanmuDataBiliBili));
            var bd = (DanmuDataBiliBili) serializer.Deserialize(s);
            D = bd.D;
        }

        [Key(0)]
        [XmlElement("d")] public iD[] D { get; set; }

        public static explicit operator DanmuDataBiliBili(BaseDanmuData[] data)
        {
            var d = data.Select(s => new iD
            {
                P = $"{s.Time},{s.Mode},{s.Size},{s.Color},{s.TimeStamp},{s.Pool},{s.Author},{s.TimeStamp}",
                Value = s.Text
            }).ToArray();
            return new DanmuDataBiliBili {D = d};
        }

        public IEnumerable<BaseDanmuData> ToDanmuDataBases()
        {
            if (D == null || D.Length == 0) return new BaseDanmuData[0];
            return D.Select(s =>
            {
                var d = s.P.Split(",");
                return new BaseDanmuData
                {
                    Time = float.Parse(d[0]),
                    Mode = int.Parse(d[1]),
                    Size = int.Parse(d[2]),
                    Color = int.Parse(d[3]),
                    TimeStamp = long.Parse(d[4]),
                    Pool = int.Parse(d[5]),
                    Author = d[6],
                    Text = s.Value
                };
            });
        }
    }

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [MessagePackObject]    // ReSharper disable once InconsistentNaming
    public class iD
    {
        [XmlAttribute("p")]
        [Key(0)]
        public string P { get; set; }

        [XmlText]
        [Key(1)]
        public string Value { get; set; }
    }
}
