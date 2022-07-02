using System.Web;
using DanMu.Models.DanMu.DPlayer.V3;
using DanMu.Models.DanMu.Generic.V1;

namespace DanMu.Models.WebResults.DPlayer.V3;

/// <summary>
///     DPlayer V3 弹幕返回数据
/// </summary>
public class DPlayerWebResult : WebResult<object[][]>
{
    public DPlayerWebResult()
    {
    }

    public DPlayerWebResult(int code) : base(code)
    {
    }

    public DPlayerWebResult(IEnumerable<GenericDanMu>? elems) : this(0)
    {
        Data = elems?.Select(s =>
        {
            var d = (DPlayerDanMu)s;
            return new object[]
            {
                d.Time,
                d.Type,
                d.Color,
                string.IsNullOrWhiteSpace(d.Author) ? "" : HttpUtility.HtmlEncode(d.Author),
                string.IsNullOrWhiteSpace(d.Text) ? "" : HttpUtility.HtmlEncode(d.Text)
            };
        }).ToArray();
    }
}