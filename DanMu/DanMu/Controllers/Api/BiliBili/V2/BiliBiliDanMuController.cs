using DanMu.Controllers.Api.Base;
using DanMu.Models.BiliBili;
using DanMu.Models.Protos.BiliBili.Dm;
using DanMu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.BiliBili.V2;

[Route("/api/bilibili/")]
[Route("/api/bilibili/v2/")]
public class BiliBiliDanMuController(BiliBiliHelp bilibili) : BiliBiliDanMuBaseController(bilibili)
{
  [HttpGet]
  [Produces("application/x-protobuf")]
  public async ValueTask<Stream?> GetProtobufDanMuFromQueryAsync([FromQuery] string? bvid, [FromQuery] int p = 1)
  {
    if (string.IsNullOrWhiteSpace(bvid)) return Stream.Null;
    return await GetProtobufDanMuAsync(bvid, p).ConfigureAwait(false);
  }

  [HttpGet("{bvid}")]
  [HttpGet("{bvid}/{p}")]
  [Produces("application/x-protobuf")]
  public ValueTask<Stream> GetProtobufDanMuAsync(string bvid, int p = 1)
  {
    return bilibili.GetDanMuStreamAsync(bvid, p);
  }

  [HttpGet("{bvid}.json")]
  [HttpGet("{bvid}/{p}.json")]
  [Produces("application/json")]
  public ValueTask<DmSegMobileReply?> GetJsonDanMuAsync(string bvid, int p = 1)
  {
    return bilibili.GetDanMuAsync(bvid, p);
  }

  [HttpGet("{bvid}.xml")]
  [HttpGet("{bvid}/{p}.xml")]
  [Produces("text/xml")]
  public async ValueTask<OldBiliBiliDanMu?> GetXmlDanMuAsync(string bvid, int p = 1)
  {
    // Response.ContentType = "text/xml";
    var a = await bilibili.GetDanMuAsync(bvid, p).ConfigureAwait(false);
    return (OldBiliBiliDanMu)a?.Elems;
  }
}