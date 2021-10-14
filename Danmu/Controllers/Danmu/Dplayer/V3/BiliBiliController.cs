using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Models.Danmus.BiliBili;
using Danmu.Models.WebResults;
using Danmu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.Dplayer.V3
{
    [Route("/api/danmu/dplayer/v3")]
    public class BiliBiliController : BiliBiliBaseController
    {
        public BiliBiliController(BiliBiliHelp bilibili) : base(bilibili)
        {
        }

        [HttpGet("bilibili")]
        public async Task<DplayerWebResult> Get([FromQuery] BiliBiliQuery query)
        {
            HttpContext.Request.Headers["Accept"] = "application/json";
            var result = await Bilibili.GetDanmuAsync(query);
            return new DplayerWebResult(result);
        }
    }
}
