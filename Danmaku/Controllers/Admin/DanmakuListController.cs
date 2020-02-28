using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model.WebResult;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Admin
{
    [Route("api/admin/danmakulist")]
    [DisableCors]
    [Authorize]
    public class DanmakuListController : DanmakuDaoBaseApiController
    {
        public DanmakuListController(DanmakuDao danmakuDao) : base(danmakuDao) { }

        [HttpGet("count")]
        public WebResult GetCount()
        {
            var count = Dao.DanmakuBaseQuery();
            return new WebResult
            {
                Data = new
                {
                    count
                }
            };
        }

        [HttpGet]
        public async Task<WebResult> Get(int page = 1, int size = 50)
        {
            var data = await Dao.DanmakuBaseQuery(page, size);
            return data.Length == 0 ? new WebResult(1) : new WebResult {Data = data};
        }
    }
}
