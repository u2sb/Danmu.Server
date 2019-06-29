using System.Collections.Generic;
using System.Linq;
using System.Web;
using Danmaku.Model;
using Danmaku.Utils;
using Danmaku.Utils.BiliBili;
using Danmaku.Utils.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Danmaku.Controllers
{
    [Route("api/dplayer")]
    [ApiController]
    public class DplayerController : ControllerBase
    {
        private readonly IBilibiliHelp _bp;
        private readonly IDanmakuDao _dd;

        public DplayerController(IDanmakuDao dd, IBilibiliHelp bp)
        {
            _dd = dd;
            _bp = bp;
        }

        // GET: api/Dplayer
        [HttpGet("v3/")]
        public WebResult Get()
        {
            string id = Request.Query["id"];
            if (string.IsNullOrEmpty(id)) return new WebResult();

            var result = _dd.Query(id);
            if (result.Count == 0) return new WebResult();
            
            return new WebResult(result)
            {
                Code = 0
            };
        }

        // POST: api/Dplayer
        [HttpPost("v3/")]
        public WebResult Post([FromBody] DanmakuModel data)
        {
            if (data == null) return new WebResult();
            data.Ip = Ip.Ip2Long(Request.Headers["X-Real-IP"]);
            data.Referer = Request.Headers["Referer"].FirstOrDefault();

            var result = _dd.Insert(new DanmakuInsert(data)) == 0 ? new WebResult() : new WebResult {Code = 0};
            return result;
        }

        [HttpGet("v3/bilibili/")]
        public WebResult BiliBili()
        {
            var request = Request.Query;
            var cid = request["cid"];
            if (string.IsNullOrEmpty(cid))
            {
                var aid = request["aid"];
                string p = request["p"];
                p = string.IsNullOrEmpty(p) ? "1" : p;

                if (!int.TryParse(p, out var page)) return null;
                cid = _bp.GetCid(aid, page).ToString();
            }

            if (cid == "0") return new WebResult();

            var ds = _bp.GetBiDanmaku(cid);
            var result = new List<object[]>();

            foreach (var d in ds)
                result.Add(new object[]
                {
                    d.Time, d.Type, d.Color, HttpUtility.HtmlEncode(d.Author),
                    HttpUtility.HtmlEncode(d.Text)
                });

            return new WebResult
            {
                Code = 0,
                Data = result
            };
        }
    }
}