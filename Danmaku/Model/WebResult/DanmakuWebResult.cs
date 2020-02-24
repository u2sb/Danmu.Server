using System.Collections.Generic;
using System.Linq;
using System.Web;
using Danmaku.Model.Danmaku;

namespace Danmaku.Model.WebResult
{
    public class DanmakuWebResult : WebResult<object[][]>
    {
        public DanmakuWebResult() { }

        public DanmakuWebResult(int code) : base(code) { }

        public DanmakuWebResult(List<DanmakuData> data)
        {
            Data = data.Select(s => new object[]
            {
                s.Time, s.Type, s.Color, HttpUtility.HtmlEncode(s.Author), HttpUtility.HtmlEncode(s.Text)
            }).ToArray();
        }
    }
}