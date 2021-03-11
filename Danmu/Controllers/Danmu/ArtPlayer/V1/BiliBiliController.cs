using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Models.Danmus.BiliBili;
using Danmu.Models.Danmus.Danmu;
using Danmu.Models.WebResults;
using Danmu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.ArtPlayer.V1
{
    [Route("/api/danmu/artplayer/v1/bilibili")]
    public class BiliBiliController : BiliBiliBaseController
    {
        public BiliBiliController(BiliBiliHelp bilibili) : base(bilibili)
        {
        }

        [HttpGet]
        [HttpGet("danmu")]
        [HttpGet("danmu.{format}")]
        public async Task<dynamic> Get([FromQuery] BiliBiliQuery query, string format)
        {
            var danmu = await Bilibili.GetDanmuAsync(query);

            if (!string.IsNullOrEmpty(format) && format.Equals("json"))
                return new WebResult<IEnumerable<ArtPlayerDanmuData>>(danmu
                    .Select(s => (ArtPlayerDanmuData) s));
            if (string.IsNullOrEmpty(format)) HttpContext.Request.Headers["Accept"] = "application/xml";
            return danmu;
        }
    }
}
