using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Models.Protos.BiliBili.Dm;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.BiliBili.V2;

[Route("/api/bilibili/")]
[Route("/api/bilibili/v2/")]
public class BiliBiliDanMuController : BiliBiliDanMuBaseController
{
  public BiliBiliDanMuController(BiliBiliHelp bilibili) : base(bilibili)
  {
  }

  [HttpGet]
  [Produces("application/x-protobuf")]
  public async Task<Stream?> GetProtobufDanMuFromQueryAsync(string? bvid, int p = 1)
  {
    if (string.IsNullOrWhiteSpace(bvid)) return Stream.Null;
    return await GetProtobufDanMuAsync(bvid, p);
  }

  [HttpGet("{bvid}")]
  [HttpGet("{bvid}/{p:int}")]
  [Produces("application/x-protobuf")]
  public async Task<Stream?> GetProtobufDanMuAsync(string bvid, int p = 1)
  {
    return await Bilibili.GetDanMuStreamAsync(bvid, p);
  }

  [HttpGet("{bvid}.json")]
  [HttpGet("{bvid}/{p:int}.json")]
  [Produces("application/json")]
  public async Task<DmSegMobileReply?> GetJsonDanMuAsync(string bvid, int p = 1)
  {
    var a = await Bilibili.GetDanMuAsync(bvid, p);
    return a;
  }

  [HttpGet("{bvid}.xml")]
  [HttpGet("{bvid}/{p:int}.xml")]
  [Produces("text/xml")]
  public async Task<OldBiliBiliDanMu?> GetXmlDanMuAsync(string bvid, int p = 1)
  {
    // Response.ContentType = "text/xml";
    var a = await Bilibili.GetDanMuAsync(bvid, p);
    return (OldBiliBiliDanMu)a?.Elems;
  }
}