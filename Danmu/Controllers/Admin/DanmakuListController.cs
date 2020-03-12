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
        public async Task<WebResult<DanmuTable[]>> GetDanmuList(int page = 1, int size = 30, bool descending = true)
        {
            var allDanmu = await DanmuDao.GetAllDanmuAsync(page, size, descending);
            return new WebResult<DanmuTable[]>(allDanmu);
        }

        /// <summary>
        ///     通过vid查询到的弹幕总数
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        [HttpGet("count" + "by" + "vid")]
        public async Task<WebResult<int>> GetCountByVid(string vid)
        {
            var count = await DanmuDao.GetDanmuByVidAsync(vid);
            return new WebResult<int>
            {
                Code = 0,
                Data = count
            };
        }

        /// <summary>
        ///     通过vid查询弹幕
        /// </summary>
        /// <returns></returns>
        [HttpGet("query" + "by" + "vid")]
        public async Task<WebResult<DanmuTable[]>> GetDanmuListByVid(string vid, int page = 1, int size = 30,
                                                                     bool descending = true)
        {
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
