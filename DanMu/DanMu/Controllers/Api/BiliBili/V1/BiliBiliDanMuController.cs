using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Models.DanMu.BiliBili.V1;
using DanMu.Models.DanMu.Generic.V1;
using DanMu.Models.WebResults;
using DanMu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.BiliBili.V1;

[Route("/api/bilibili/")]
[Route("/api/bilibili/v1/")]
public class BiliBiliDanMuController : BiliBiliDanMuBaseController
{
    public BiliBiliDanMuController(BiliBiliHelp bilibili) : base(bilibili)
    {
    }

    [HttpGet]
    [HttpGet(".{format?}")]
    [HttpGet("{id}/{p:int?}")]
    [HttpGet("{id}.{format?}/{p:int?}")]
    [HttpGet("{id}/{p:int}.{format?}")]
    public async Task<dynamic> GetDanMu([FromQuery] BQuery query, string? id, int? p, string? format)
    {
        p ??= 1;
        var danMu = await Bilibili.GetGenericDanMuAsync(query, id, (int)p);
        if (!string.IsNullOrEmpty(format) && format.Equals("json"))
            return new WebResult<IEnumerable<GenericDanMu>?>(danMu);

        if (string.IsNullOrEmpty(format)) HttpContext.Request.Headers["Accept"] = "application/xml";
        return (BiliBiliDanMu)danMu;
    }
}