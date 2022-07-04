using System.Net;
using DanMu.Controllers.Api.Base;
using DanMu.Models.DanMu.ArtPlayer.V1;
using DanMu.Models.DanMu.BiliBili.V1;
using DanMu.Models.DanMu.Generic.V1;
using DanMu.Models.WebResults;
using DanMu.Utils.Dao.Danmu;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.ArtPlayer.V1;

[Route("/api/artplayer/v1/")]
public class ArtPlayerDanMuController : DanMuController
{
    public ArtPlayerDanMuController(DanmuTableDao danmu) : base(danmu)
    {
    }

    [HttpGet]
    [HttpGet(".{format?}")]
    [HttpGet("{id}.{format?}")]
    [HttpGet("{id}.{format?}/{p:int?}")]
    [HttpGet("{id}/{p:int}.{format?}")]
    public async Task<dynamic> GetAsync(string? id, string? format)
    {
        id ??= Request.Query["id"];
        format ??= Request.Query["format"];

        if (!string.IsNullOrEmpty(format) && format.Equals("json"))
            return string.IsNullOrEmpty(id)
                ? new WebResult(1)
                : new WebResult<IEnumerable<ArtPlayerDanMu>?>(
                    (await Danmu.GetDanmuByVidAsync(id)).Select(s => (ArtPlayerDanMu)s));

        if (string.IsNullOrEmpty(format) || format == "xml") HttpContext.Request.Headers["Accept"] = "application/xml";
        return (BiliBiliDanMu)await Danmu.GetDanmuByVidAsync(id);
    }

    [HttpPost]
    public async Task<WebResult> PostAsync([FromBody] ArtPlayerDanMuIn data)
    {
        if (string.IsNullOrWhiteSpace(data.Id) || string.IsNullOrWhiteSpace(data.Text))
            return new WebResult(1);
        data.Ip = IPAddress.TryParse(Request.Headers["X-Real-IP"], out var ip)
            ? ip
            : Request.HttpContext.Connection.RemoteIpAddress;
        data.Referer ??= Request.Headers["Referer"].FirstOrDefault();

        var result = await Danmu.InsertDanMuAsync((GenericDanMu)data, data.Id, data.Referer, data.Ip);
        return new WebResult(result ? 0 : 1);
    }
}