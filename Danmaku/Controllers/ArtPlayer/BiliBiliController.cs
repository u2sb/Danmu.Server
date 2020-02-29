using System.Threading.Tasks;
using Danmaku.Model.Danmaku;
using Danmaku.Model.WebResult;
using Danmaku.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.ArtPlayer
{
    [Route("api/artplayer")]
    [FormatFilter]
    [ApiController]
    public class BiliBiliController : ControllerBase
    {
        private readonly BiliBiliHelp _biliBili;

        public BiliBiliController(BiliBiliHelp biliBiliHelp)
        {
            _biliBili = biliBiliHelp;
        }

        [HttpGet("bilibili")]
        [HttpGet("bilibili.{format}")]
        public async Task<dynamic> Get(string format)
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

            if (string.IsNullOrWhiteSpace(cid)) return null;

            if (date.Length > 0)
            {
                var bd = _biliBili.GetBiDanmakuStream(cid, date);
                if (format == "json")
                    return new WebResult
                    {
                        Data = bd.ToArtPlayerDanmakus()
                    };

                return bd;
            }

            var rawStream = _biliBili.GetBiDanmakuRaw(cid);

            if (format == "json")
            {
                var b = new BilibiliDanmakuData(await rawStream);
                var danmakus = b.ToArtPlayerDanmakus();
                return new WebResult
                {
                    Data = danmakus
                };
            }

            return await rawStream;
        }
    }
}
