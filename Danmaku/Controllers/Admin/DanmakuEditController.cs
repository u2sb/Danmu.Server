using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model.Danmaku;
using Danmaku.Model.WebResult;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Admin
{
    [Route("api/admin/danmakuedit")]
    [DisableCors]
    [Authorize]
    public class DanmakuEditController : DanmakuDaoBaseApiController
    {
        public DanmakuEditController(DanmakuDao danmakuDao) : base(danmakuDao) { }

        [HttpGet("get")]
        public async Task<WebResult> GetDanmaku(string id)
        {
            var data = await Dao.DanmakuBaseQuery(id);
            return data == null
                    ? new WebResult(1)
                    : new WebResult
                    {
                        Data = data
                    };
        }

        [HttpPost("edit")]
        public async Task<WebResult> EditDanmaku(DanmakuDataBase data)
        {
            var result = await Dao.DanmakuEdit(data.Id, data.DplayerDanmaku.Time, data.DplayerDanmaku.Type,
                    data.DplayerDanmaku.Color,
                    data.DplayerDanmaku.Text);
            return result == null ? new WebResult(1) : new WebResult {Data = result};
        }

        [HttpGet("delete")]
        public async Task<WebResult> DeleteDanmaku(string id)
        {
            var result = await Dao.DanmakuDelete(id);
            return result ? new WebResult(0) : new WebResult(1);
        }
    }
}
