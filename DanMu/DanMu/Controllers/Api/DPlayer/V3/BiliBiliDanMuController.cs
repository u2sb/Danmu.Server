using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Models.WebResults.DPlayer.V3;
using DanMu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.DPlayer.V3;

[Route("/api/dplayer/bilibili/")]
[Route("/api/dplayer/v1/bilibili/")]
[Route("/api/dplayer/v2/bilibili/")]
[Route("/api/dplayer/v3/bilibili/")]
public class BiliBiliDanMuController : BiliBiliDanMuBaseController
{
    public BiliBiliDanMuController(BiliBiliHelp bilibili) : base(bilibili)
    {
    }

    [HttpGet("danmu")]
    [HttpGet("{id}/{p:int?}")]
    public async Task<DPlayerWebResult> Get([FromQuery] BQuery query, string? id, int p = 1)
    {
        var danMu = await Bilibili.GetGenericDanMuAsync(query, id, p);
        return new DPlayerWebResult(danMu);
    }
}