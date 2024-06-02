using DanMu.Models.ArtPlayer;
using DanMu.Models.BiliBili;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.ArtPlayer;

[Route("/api/art/bilibili/")]
[FormatFilter]
[ApiController]
public class BiliBiliDanMuController(BiliBiliHelp bilibili)
{
  #region MemoryPack弹幕

  [HttpGet("v2/{bvid}")]
  [HttpGet("v2/{bvid}/{p}")]
  [Produces("application/x-memorypack")]
  public async ValueTask<List<ArtPlayerDm>> GetMemoryPackDanMuAsync(string bvid, int p = 1)
  {
    var dm = await bilibili.GetDanMuAsync(bvid, p);

    return ArtPlayerDm.FromBilibiliDanMu(dm?.Elems);
  }

  #endregion

  #region Json弹幕

  [HttpGet("v2/{bvid}.json")]
  [HttpGet("v2/{bvid}/{p}.json")]
  [Produces("application/json")]
  public async ValueTask<List<ArtPlayerDm>> GetJsonDanMuAsync(string bvid, int p = 1)
  {
    var dm = await bilibili.GetDanMuAsync(bvid, p);

    return  ArtPlayerDm.FromBilibiliDanMu(dm?.Elems);
  }

  #endregion

  #region XML弹幕

  [HttpGet("v1/{bvid}")]
  [HttpGet("v1/{bvid}/{p}")]
  [HttpGet("v1/{bvid}.xml")]
  [HttpGet("v1/{bvid}/{p}.xml")]
  [Produces("text/xml")]
  public async ValueTask<OldBiliBiliDanMu> GetXmlDanMuAsync(string bvid, int p = 1)
  {
    var a = await bilibili.GetDanMuAsync(bvid, p).ConfigureAwait(false);
    return (OldBiliBiliDanMu)a?.Elems;
  }

  [HttpGet("v1")]
  [Produces("text/xml")]
  public async ValueTask<OldBiliBiliDanMu> GetDanXmlMuFromQueryAsync([FromQuery] string? bvid, [FromQuery] int p = 1)
  {
    if (string.IsNullOrWhiteSpace(bvid)) return new OldBiliBiliDanMu();

    return await GetXmlDanMuAsync(bvid, p).ConfigureAwait(false);
  }

  #endregion
}