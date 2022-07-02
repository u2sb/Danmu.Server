using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Models.DanMu.ArtPlayer.V1;
using DanMu.Models.DanMu.BiliBili.V1;
using DanMu.Models.WebResults;
using DanMu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.ArtPlayer.V1;

[Route("/api/artplayer/bilibili/")]
[Route("/api/artplayer/v1/bilibili/")]
public class BiliBiliDanMuController : BiliBiliDanMuBaseController
{
    public BiliBiliDanMuController(BiliBiliHelp bilibili) : base(bilibili)
    {
    }

    [HttpGet]
    [HttpGet(".{format?}")]
    [HttpGet("{id}.{format?}")]
    [HttpGet("{id}.{format?}/{p:int?}")]
    [HttpGet("{id}/{p:int}.{format?}")]
    public async Task<dynamic> Get([FromQuery] BQuery query, string? id, int? p, string? format)
    {
        p ??= 1;
        var danMu = await Bilibili.GetGenericDanMuAsync(query, id, (int)p);
        if (!string.IsNullOrEmpty(format) && format.Equals("json"))
            return new WebResult<IEnumerable<ArtPlayerDanMu>?>(danMu?.Select(s => (ArtPlayerDanMu)s));
        if (string.IsNullOrEmpty(format) || format == "xml") HttpContext.Request.Headers["Accept"] = "application/xml";
        return (BiliBiliDanMu)danMu;
    }
}