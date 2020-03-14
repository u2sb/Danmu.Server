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
        ///     获取弹幕数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public async Task<WebResult<int>> GetCount(string vid = null)
        {
            if (string.IsNullOrEmpty(vid))
            {
                var allCount = await DanmuDao.GetAllDanmuAsync();
                return new WebResult<int>
                {
                    Code = 0,
                    Data = allCount
                };
            }

            var count = await DanmuDao.GetDanmuByVidAsync(vid);
            return new WebResult<int>
            {
                Code = 0,
                Data = count
            };
        }

        /// <summary>
        ///     获取弹幕
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<WebResult<DanmuTable[]>> GetDanmuList(string vid = null, int page = 1, int size = 30,
                                                                bool descending = true)
        {
            if (string.IsNullOrEmpty(vid))
            {
                var allDanmu = await DanmuDao.GetAllDanmuAsync(page, size, descending);
                return new WebResult<DanmuTable[]>(allDanmu);
            }

            var danmu = await DanmuDao.GetDanmuByVidAsync(vid, page, size, descending);
            return new WebResult<DanmuTable[]>(danmu);
        }

        /// <summary>
        ///     获取vid集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("vids")]
        public async Task<WebResult<string[]>> GetVidList()
        {
            var vids = await VideoDao.GetVidsAsync();
            return new WebResult<string[]>(vids);
        }
    }
}
