using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Admin
{
    [Route("/api/admin/danmulist")]
    public class DanmuListController : AdminBaseController
    {
        public DanmuListController(DanmuDao danmuDao, VideoDao videoDao) : base(danmuDao, videoDao) { }

        /// <summary>
        ///     获取全部弹幕数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public async Task<WebResult<int>> GetCount()
        {
            var count = await DanmuDao.GetAllDanmuAsync();
            return new WebResult<int>
            {
                Code = 0,
                Data = count
            };
        }

        /// <summary>
        ///     获取全部弹幕
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<WebResult<DanmuTable[]>> GetDanmuList(int page, int size, bool descending = true)
        {
            var allDanmu = await DanmuDao.GetAllDanmuAsync(page, size, descending);
            return new WebResult<DanmuTable[]>(allDanmu);
        }
    }
}
