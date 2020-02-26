using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model.Danmaku;
using Danmaku.Model.WebResult;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.ArtPlayer.V1
{
    [Route("api/artplayer/v1")]
    [ApiController]
    public class ArtPlayerController : DanmakuDaoBaseApiController
    {
        public ArtPlayerController(DanmakuDao danmakuDao) : base(danmakuDao) { }

        [HttpGet]
        public async Task<BilibiliDanmakuData> Get(string id)
        {
            HttpContext.Request.Headers["Accept"] = "application/xml";
            return string.IsNullOrEmpty(id) ? null : new BilibiliDanmakuData(await Dao.DanmakuQuery(id));
        }

        // POST: api/artplayer/v1/
        [HttpPost]
        public async Task<DanmakuWebResult> Post([FromBody] DanmakuDataInsert data)
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
