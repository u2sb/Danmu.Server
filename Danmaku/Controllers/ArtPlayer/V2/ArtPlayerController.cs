using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model.Danmaku;
using Danmaku.Model.WebResult;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.ArtPlayer.V2
{
    [Route("api/artplayer/v2")]
    [FormatFilter]
    [ApiController]
    public class ArtPlayerController : DanmakuDaoBaseApiController
    {
        public ArtPlayerController(DanmakuDao danmakuDao) : base(danmakuDao) { }

        [HttpGet]
        [HttpGet("{id}.{format?}")]
        public async Task<dynamic> Get(string id, string format)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Request.Query["id"];
                if (string.IsNullOrEmpty(id)) return new WebResult(1);
            }

            var d = await Dao.DanmakuQuery(id);
            if (format == "json")
            {
                var a = d.Select(s => new ArtPlayerDanmaku(s));
                return new WebResult
                {
                    Data = a
                };
            }

            if (string.IsNullOrEmpty(format)) HttpContext.Request.Headers["Accept"] = "application/xml";

            return new BilibiliDanmakuData(d);
        }

        // POST: api/artplayer/v1/
        [HttpPost]
        public async Task<DanmakuWebResult> Post([FromBody] DplayerDanmakuInsert data)
        {
            if (string.IsNullOrWhiteSpace(data.Id) || string.IsNullOrWhiteSpace(data.Text))
                return new DanmakuWebResult(1);
            data.Ip = IPAddress.TryParse(Request.Headers["X-Real-IP"], out var ip)
                    ? ip
                    : Request.HttpContext.Connection.RemoteIpAddress;
            data.Referer = Request.Headers["Referer"].FirstOrDefault();

            var result = await Dao.DanmakuInsert(data);
            return result ? new DanmakuWebResult(0) : new DanmakuWebResult(1);
        }
    }
}
