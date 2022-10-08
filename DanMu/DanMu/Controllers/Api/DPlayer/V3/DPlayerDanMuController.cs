using System.Net;
using DanMu.Controllers.Api.Base;
using DanMu.Models.DanMu.DPlayer.V3;
using DanMu.Models.DanMu.Generic.V1;
using DanMu.Models.WebResults.DPlayer.V3;
using DanMu.Utils.Dao.Danmu;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.DPlayer.V3;

[Route("/api/dplayer/v3/")]
public class DPlayerDanMuController : DanMuController
{
    public DPlayerDanMuController(DanmuTableDao danmu) : base(danmu)
    {
    }

    [HttpGet]
    public async Task<DPlayerWebResult> GetAsync([FromQuery] string id)
    {
        return string.IsNullOrEmpty(id)
            ? new DPlayerWebResult(1)
            : new DPlayerWebResult(await Danmu.GetDanmuByVidAsync(id));
    }

    [HttpPost]
    public async Task<DPlayerWebResult> PostAsync([FromBody] DPlayerDanMuIn data)
    {
        if (string.IsNullOrWhiteSpace(data.Id) || string.IsNullOrWhiteSpace(data.Text))
            return new DPlayerWebResult(1);
        data.Ip = IPAddress.TryParse(Request.Headers["X-Real-IP"], out var ip)
            ? ip
            : Request.HttpContext.Connection.RemoteIpAddress;
        data.Referer ??= Request.Headers["Referer"].FirstOrDefault();

        var result = await Danmu.InsertDanMuAsync((GenericDanMu)data, data.Id, data.Referer, data.Ip);
        return new DPlayerWebResult(result ? 0 : 1);
    }
}