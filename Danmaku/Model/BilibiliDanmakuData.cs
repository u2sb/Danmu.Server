using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Danmaku.Model
{
    [SerializableAttribute]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("i", Namespace = "", IsNullable = false)]
    public class BilibiliDanmakuData
    {
        public BilibiliDanmakuData() { }
        public BilibiliDanmakuData(List<DanmakuData> ds)
        {

            D = ds.Select(d => new iD
            {
                P = $"{d.Time},1,25,{d.Color},1512931469,{d.Type},354b5ade,4028451968",
                Value = d.Text
            }).ToArray();

        }

        [XmlElement("d")] public iD[] D { get; set; }

        /// <summary>
        ///     转通用Data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DanmakuData> ToDanmakuDataList()
        {
            return D.Select(s =>
            {
                var d = s.P.Split(",");
                return new DanmakuData
                {
                    Time = float.Parse(d[0]),
                    Color = int.Parse(d[3]),
                    Type = int.Parse(d[5]),
                    Author = "",
                    Text = s.Value
                };
            });
        }

        public string ToXml()
        {
            using MemoryStream ms = new MemoryStream();
            XmlSerializer x = new XmlSerializer(typeof(BilibiliDanmakuData));
            x.Serialize(ms,this);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }

    [SerializableAttribute]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    // ReSharper disable once InconsistentNaming
    public class iD
    {
        [XmlAttribute("p")] public string P { get; set; }

        [XmlText] public string Value { get; set; }
    }
}