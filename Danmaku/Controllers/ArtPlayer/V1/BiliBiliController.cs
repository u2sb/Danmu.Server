using System.Threading.Tasks;
using Danmaku.Model;
using Danmaku.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.ArtPlayer.V1
{
    [Route("api/artplayer/v1")]
    [ApiController]
    public class BiliBiliController : ControllerBase
    {
        private readonly IBiliBiliHelp _biliBili;

        public BiliBiliController(IBiliBiliHelp biliBiliHelp)
        {
            _biliBili = biliBiliHelp;
        }

        // GET: api/dplayer/v3/bilibili
        [HttpGet("bilibili")]
        public async Task<string> Get()
        {
            var request = Request.Query;

            string[] date = request["date"];
            var cid = request["cid"].ToString();
            if (string.IsNullOrEmpty(cid))
            {
                var aid = request["aid"];
                if (string.IsNullOrEmpty(aid)) return new DanmakuWebResult(1);
                string p = request["p"];
                p = string.IsNullOrEmpty(p) ? "1" : p;
                if (!int.TryParse(p, out var page)) return new DanmakuWebResult(1);
                cid = (await _biliBili.GetCid(aid, page)).ToString();
            }

            return string.IsNullOrWhiteSpace(cid)
                    ? null
                    : await _biliBili.GetBiDanmakuRaw(cid);

        }
    }
}