using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Danmaku.Model
{
    public class DanmakuWebResult
    {
        public DanmakuWebResult()
        {
        }

        public DanmakuWebResult(List<DanmakuData> data)
        {
            Data = data.Select(s => new object[] {s.Time, s.Type, s.Color, s.Author, s.Text}).ToList();
        }


        /// <summary>
        ///     代码，0正常 1错误
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        ///     数据
        /// </summary>
        public List<object[]> Data { get; set; }
    }

    public class DanmakuData
    {
        public float Time { get; set; }

        public int Type { get; set; }

        public int Color { get; set; }

        [MaxLength(16)] public string Author { get; set; }

        [MaxLength(255)] public string Text { get; set; }
    }
}