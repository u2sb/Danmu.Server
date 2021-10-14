using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Danmu.Models.Danmus.Danmu;

namespace Danmu.Models.Danmus.BiliBili
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("i", Namespace = "", IsNullable = false)]
    public class BiliBiliDanmuData
    {
        public BiliBiliDanmuData() { }

        public BiliBiliDanmuData(Stream s)
        {
            var serializer = new XmlSerializer(typeof(BiliBiliDanmuData));
            var bd = (BiliBiliDanmuData) serializer.Deserialize(s);
            if (bd != null) D = bd.D;
        }

        [XmlElement("d")] public iD[] D { get; set; }

        public static explicit operator BiliBiliDanmuData(BaseDanmuData[] data)
        {
            var d = data.Select(s => new iD
            {
                P = $"{s.Time},{s.Mode},{s.Size},{s.Color},{s.TimeStamp},{s.Pool},{s.Author},{s.TimeStamp}",
                Value = s.Text
            }).ToArray();
            return new BiliBiliDanmuData {D = d};
        }

        public IEnumerable<BaseDanmuData> ToBaseDanmuDatas()
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
    public class iD
    {
        [XmlAttribute("p")]
        public string P { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
