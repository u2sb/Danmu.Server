using System.Threading.Tasks;
using Danmu.Model.WebResult;
using Danmu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Other
{
    [Route("api/other/query" + "aid")]
    [ApiController]
    public class QueryAidController : ControllerBase
    {
        private readonly BiliBiliHelp _bilibili;

        public QueryAidController(BiliBiliHelp bilibil)
        {
            _bilibili = bilibil;
        }

        [HttpGet]
        public async Task<WebResult> QueryAid(string bvid)
        {
            var aid = await _bilibili.GetAidByBvidAsync(bvid);
            return string.IsNullOrEmpty(bvid) || aid == 0 ? new WebResult(1) : new WebResult(0)
            {
                Data = new
                {
                    Aid = aid,
                    PageList = (await _bilibili.GetBiliBiliPageAsync(bvid)).Data
                }
            };
        }
    }
}
