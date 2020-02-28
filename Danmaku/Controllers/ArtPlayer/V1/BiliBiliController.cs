using System.IO;
using System.Threading.Tasks;
using Danmaku.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.ArtPlayer.V1
{
    [Route("api/artplayer/v1")]
    [ApiController]
    public class BiliBiliController : ControllerBase
    {
        private readonly BiliBiliHelp _biliBili;

        public BiliBiliController(BiliBiliHelp biliBiliHelp)
        {
            _biliBili = biliBiliHelp;
        }

        [HttpGet("bilibili")]
        public async Task<Stream> Get()
        {
            var request = Request.Query;

            string[] date = request["date"];
            var cid = request["cid"].ToString();
            if (string.IsNullOrEmpty(cid))
            {
                var aid = request["aid"];
                if (string.IsNullOrEmpty(aid)) return null;
                string p = request["p"];
                p = string.IsNullOrEmpty(p) ? "1" : p;
                if (!int.TryParse(p, out var page)) return null;
                cid = (await _biliBili.GetCid(aid, page)).ToString();
            }

            return string.IsNullOrWhiteSpace(cid)
                    ? null
                    : await _biliBili.GetBiDanmakuRaw(cid);
        }
    }
}