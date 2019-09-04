using Danmaku.Model;
using Danmaku.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Dplayer.V3
{
    [Route("api/dplayer/v3/bilibili")]
    [ApiController]
    public class BiliBiliController : ControllerBase
    {
        private readonly IBiliBiliHelp _biliBili;

        public BiliBiliController(IBiliBiliHelp biliBiliHelp)
        {
            _biliBili = biliBiliHelp;
        }


        // GET: api/dplayer/v3/bilibili
        [HttpGet]
        public DanmakuWebResult Get()
        {
            var request = Request.Query;

            string[] date = request["date"];
            var cid = request["cid"];
            if (string.IsNullOrEmpty(cid))
            {
                var aid = request["aid"];
                string p = request["p"];
                p = string.IsNullOrEmpty(p) ? "1" : p;
                if (!int.TryParse(p, out var page)) return null;
                cid = _biliBili.GetCid(aid, page).ToString();
            }

            return cid == 0
                ? new DanmakuWebResult(1)
                : date.Length == 0
                    ? new DanmakuWebResult(_biliBili.GetBiDanmaku(cid))
                    : new DanmakuWebResult(_biliBili.GetBiDanmaku(cid, date));
        }
    }
}