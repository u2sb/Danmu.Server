using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model.Danmaku;
using Danmaku.Model.WebResult;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Dplayer.V3
{
    [Route("api/dplayer/v3")]
    [ApiController]
    public class DplayerController : DanmakuDaoBaseApiController
    {
        public DplayerController(DanmakuDao danmakuDao) : base(danmakuDao) { }

        // GET: api/dplayer/v3/
        [HttpGet]
        public async Task<string> Get(string id)
        {
            return string.IsNullOrEmpty(id)
                    ? new DanmakuWebResult(1)
                    : new DanmakuWebResult(await Dao.DanmakuQuery(id));
        }

        // POST: api/dplayer/v3/
        [HttpPost]
        public async Task<string> Post([FromBody] DplayerDanmakuInsert data)
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
