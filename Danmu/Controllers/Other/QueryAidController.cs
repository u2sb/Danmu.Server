using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.WebResult;
using Danmu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Other
{
    [Route("/api/other/bilibili")]
    [ApiController]
    public class QueryAidController : BiliBiliBaseController
    {
        public QueryAidController(BiliBiliHelp bilibili) : base(bilibili) { }

        [HttpGet("query" + "aid")]
        public async Task<WebResult> QueryAid(string bvid, int? aid)
        {
            var a = await Bilibili.GetBvidInfoAsync(bvid, aid ?? 0);

            return a.Code == 0
                    ? new WebResult(0)
                    {
                        Data = new
                        {
                            a.Data.Aid,
                            a.Data.Bvid,
                            PageList = (await Bilibili.GetBiliBiliPageAsync(a.Data.Bvid)).Data
                        }
                    }
                    : new WebResult(1);
        }
    }
}
