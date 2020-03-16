using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Admin
{
    [Route("/api/admin/danmuedit/")]
    public class DanmuEditController : AdminBaseController
    {
        public DanmuEditController(DanmuDao danmuDao, VideoDao videoDao) : base(danmuDao, videoDao) { }

        [HttpGet()]
        public async Task<WebResult<DanmuTable>> Get(string id)
        {
            var danmu = await DanmuDao.QueryDanmuByIdAsync(id);
            return new WebResult<DanmuTable>(danmu);
        }

        [HttpPost("edit")]
        public async Task<WebResult<DanmuTable>> EditDanmu(DanmuTable data)
        {
            var result = await DanmuDao.EditDanmuAsync(data.Id, data.Data.Time, data.Data.Mode, data.Data.Color,
                    data.Data.Text, data.IsDelete);
            return new WebResult<DanmuTable>(result);
        }

        [HttpGet("delete")]
        public async Task<WebResult> DeleteDanmu(string id)
        {
            var result = await DanmuDao.DeleteDanmuAsync(id);
            return new WebResult(result ? 0 : 1);
        }
    }
}
