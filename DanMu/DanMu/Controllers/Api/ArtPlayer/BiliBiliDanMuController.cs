using System.Net.Mime;
using DanMu.Models.ArtPlayer;
using DanMu.Models.BiliBili;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.ArtPlayer;

[FormatFilter]
[ApiController]
[Route("/api/art/bilibili/")]
public class BiliBiliDanMuController(BiliBiliHelp bilibili) : ControllerBase
{
  #region MemoryPack 弹幕   MessagePack 弹幕

  [HttpGet("v2/{bvid}")]
  [HttpGet("v2/{bvid}/{p:int}")]
  public async ValueTask<List<ArtPlayerDm>> GetMemoryPackDanMuAsync(string bvid, int p = 1)
  {
    HttpContext.Response.ContentType = HttpContext.Request.ContentType;
    var dm = await bilibili.GetDanMuAsync(bvid, p);
    return ArtPlayerDm.FromBilibiliDanMu(dm?.Elems);
  }

  #endregion

  #region Json弹幕

  [HttpGet("v2/{bvid}.json")]
  [HttpGet("v2/{bvid}/{p}.json")]
  [Produces(MediaTypeNames.Application.Json)]
  public async ValueTask<List<ArtPlayerDm>> GetJsonDanMuAsync(string bvid, int p = 1)
  {
    var dm = await bilibili.GetDanMuAsync(bvid, p);

    return ArtPlayerDm.FromBilibiliDanMu(dm?.Elems);
  }

  #endregion

  #region XML弹幕

  [HttpGet("v1/{bvid}")]
  [HttpGet("v1/{bvid}/{p}")]
  [HttpGet("v1/{bvid}.xml")]
  [HttpGet("v1/{bvid}/{p}.xml")]
  [Produces(MediaTypeNames.Application.Xml)]
  public async ValueTask<OldBiliBiliDanMu> GetXmlDanMuAsync(string bvid, int p = 1)
  {
    var a = await bilibili.GetDanMuAsync(bvid, p).ConfigureAwait(false);
    return (OldBiliBiliDanMu)a?.Elems;
  }

  [HttpGet("v1")]
  [Produces(MediaTypeNames.Application.Xml)]
  public async ValueTask<OldBiliBiliDanMu> GetDanXmlMuFromQueryAsync([FromQuery] string? bvid, [FromQuery] int p = 1)
  {
    if (string.IsNullOrWhiteSpace(bvid)) return new OldBiliBiliDanMu();
    return await GetXmlDanMuAsync(bvid, p).ConfigureAwait(false);
  }

  #endregion
}