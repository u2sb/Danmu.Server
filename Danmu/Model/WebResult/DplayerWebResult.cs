using System.Linq;
using System.Web;
using Danmu.Model.Danmu.DanmuData;
using MessagePack;

namespace Danmu.Model.WebResult
{
    [MessagePackObject]
    public class DplayerWebResult : WebResult<object[][]>
    {
        public DplayerWebResult() { }
        public DplayerWebResult(int code) : base(code) { }

        public DplayerWebResult(BaseDanmuData[] data) : this(0)
        {
            Data = data.Select(s =>
            {
                var d = (DplayerDanmuData) s;
                return new object[]
                {
                    d.Time, d.Type, d.Color, HttpUtility.HtmlEncode(d.Author), HttpUtility.HtmlEncode(d.Text)
                };
            }).ToArray();
        }
    }
}
