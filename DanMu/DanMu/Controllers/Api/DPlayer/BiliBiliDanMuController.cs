using System.Net.Mime;
using DanMu.Models.DPlayer;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.DPlayer;

[FormatFilter]
[ApiController]
[Route("/api/dp/bilibili/")]
public class BiliBiliDanMuController(BiliBiliHelp bilibili) : ControllerBase
{
  #region MessagePack 弹幕

  [HttpGet("v3/{bvid}")]
  [HttpGet("v3/{bvid}/{p:int}")]
  [Produces("application/x-msgpack")]
  public async ValueTask<DPlayerDmResult> GetMemoryPackDanMuAsync(string bvid, int p = 1)
  {
    var dm = await bilibili.GetDanMuAsync(bvid, p);
    var dpDm = DPlayerDm.FromBilibiliDanMu(dm?.Elems);

    return new DPlayerDmResult
    {
      Data = dpDm
    };
  }

  #endregion

  #region Json 弹幕

  [HttpGet("v3/{bvid}.json")]
  [HttpGet("v3/{bvid}/{p}.json")]
  [Produces(MediaTypeNames.Application.Json)]
  public async ValueTask<DPlayerDmResult> GetJsonDanMuAsync(string bvid, int p = 1)
  {
    var dm = await bilibili.GetDanMuAsync(bvid, p);
    var dpDm = DPlayerDm.FromBilibiliDanMu(dm?.Elems);

    return new DPlayerDmResult
    {
      Data = dpDm
    };
  }

  [HttpGet("v3")]
  [Produces(MediaTypeNames.Application.Json)]
  public DPlayerDmResult GetNullJsonDanMuAsync()
  {
    return new DPlayerDmResult();
  }

  #endregion
}