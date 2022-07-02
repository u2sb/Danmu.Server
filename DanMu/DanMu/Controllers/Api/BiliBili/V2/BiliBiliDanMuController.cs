using Bilibili.Community.Service.Dm.V1;
using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Models.WebResults;
using DanMu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.BiliBili.V2;

[Route("/api/bilibili/v2/")]
public class BiliBiliDanMuController : BiliBiliDanMuBaseController
{
    public BiliBiliDanMuController(BiliBiliHelp bilibili) : base(bilibili)
    {
    }

    [HttpGet]
    [HttpGet("{id}/{p:int?}")]
    public async Task<dynamic> GetDanMu([FromQuery] BQuery query, string? id, int? p, string? format)
    {
        p ??= 1;
        var danMu = await Bilibili.GetGenericDanMuAsync(query, id, (int)p);

        return new WebResult<IEnumerable<DanmakuElem>?>(danMu?.Select(s => (DanmakuElem)s));
    }
}