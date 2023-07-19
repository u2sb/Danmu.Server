using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.BiliBili.V1;

[Route("/api/bilibili/v1/")]
public class BiliBiliDanMuController : BiliBiliDanMuBaseController
{
  public BiliBiliDanMuController(BiliBiliHelp bilibili) : base(bilibili)
  {
  }

  [HttpGet("{bvid}")]
  [HttpGet("{bvid}/{p:int}")]
  [HttpGet("{bvid}.xml")]
  [HttpGet("{bvid}/{p:int}.xml")]
  [Produces("text/xml")]
  public async Task<OldBiliBiliDanMu?> GetXmlDanMu(string bvid, int p = 1)
  {
    var a = await Bilibili.GetDanMuAsync(bvid, p);
    return (OldBiliBiliDanMu)a?.Elems;
  }

  [HttpGet]
  [Produces("text/xml")]
  public async Task<OldBiliBiliDanMu?> GetDanXmlMuFromQuery([FromQuery] string? bvid, [FromQuery] int p = 1)
  {
    if (string.IsNullOrWhiteSpace(bvid))
    {
      return new OldBiliBiliDanMu();
    }

    return await GetXmlDanMu(bvid, p);
  }
}