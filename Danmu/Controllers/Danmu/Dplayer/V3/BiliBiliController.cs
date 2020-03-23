using System.Linq;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.WebResult;
using Danmu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.Dplayer.V3
{
    [Route("/api/danmu/dplayer/v3")]
    public class BiliBiliController : BiliBiliBaseController
    {
        public BiliBiliController(BiliBiliHelp bilibili) : base(bilibili) { }

        [HttpGet("bilibili")]
        public async Task<DplayerWebResult> Get(int cid, int aid, string bvid, int p)
        {
            string[] date = Request.Query["date"];
            HttpContext.Request.Headers["Accept"] = "application/json";
            var result = await Bilibili.GetDanmuAsync(cid, aid, bvid, p, date);
            return new DplayerWebResult(result.ToDanmuDataBases().ToArray());
        }
    }
}
