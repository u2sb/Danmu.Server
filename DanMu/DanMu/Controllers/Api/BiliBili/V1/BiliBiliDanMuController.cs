using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.BiliBili.V1;

[Route("/api/bilibili/v1/")]
public class BiliBiliDanMuController(BiliBiliHelp bilibili) : BiliBiliDanMuBaseController(bilibili)
{
  [HttpGet("{bvid}")]
  [HttpGet("{bvid}/{p}")]
  [HttpGet("{bvid}.xml")]
  [HttpGet("{bvid}/{p}.xml")]
  [Produces("text/xml")]
  public async ValueTask<OldBiliBiliDanMu?> GetXmlDanMu(string bvid, int p = 1)
  {
    var a = await bilibili.GetDanMuAsync(bvid, p).ConfigureAwait(false);
    return (OldBiliBiliDanMu)a?.Elems;
  }

  [HttpGet]
  [Produces("text/xml")]
  public async ValueTask<OldBiliBiliDanMu?> GetDanXmlMuFromQuery([FromQuery] string? bvid, [FromQuery] int p = 1)
  {
    if (string.IsNullOrWhiteSpace(bvid)) return new OldBiliBiliDanMu();

    return await GetXmlDanMu(bvid, p).ConfigureAwait(false);
  }
}