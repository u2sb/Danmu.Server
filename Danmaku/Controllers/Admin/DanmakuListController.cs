using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model.WebResult;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Admin
{
    [Route("api/admin/danmakulist")]
    [Authorize]
    public class DanmakuListController : DanmakuDaoBaseApiController
    {
        public DanmakuListController(DanmakuDao danmakuDao) : base(danmakuDao) { }

        [HttpGet("count")]
        public WebResult GetCount()
        {
            var count = Dao.DanmakuBaseQuery();
            return new WebResult {Data = count};
        }

        [HttpGet]
        public async Task<WebResult> Get(int page = 1, int size = 50)
        {
            var data = await Dao.DanmakuBaseQuery(page, size);
            return new WebResult {Data = data};
        }
    }
}
