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
            var count = 0;
            count = string.IsNullOrEmpty(vid)
                    ? await DanmuDao.GetAllDanmuAsync()
                    : await DanmuDao.GetDanmuByVidAsync(vid);
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
            var danmu = string.IsNullOrEmpty(vid)
                    ? await DanmuDao.GetAllDanmuAsync(page, size, descending)
                    : await DanmuDao.GetDanmuByVidAsync(vid, page, size, descending);
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

        /// <summary>
        ///     日期筛选
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        [HttpGet("date" + "select")]
        public async Task<WebResult<DanmuTable[]>> DateSelect(int page = 1, int size = 30, string startDate = null,
                                                              string endDate = null, bool descending = true)
        {
            var result = await DanmuDao.DateSelectAsync(page, size, startDate, endDate);
            return new WebResult<DanmuTable[]>(result);
        }

        /// <summary>
        ///     基础查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("base" + "select")]
        public async Task<WebResult<DanmuTable[]>> DanmuBasesSelect(int page = 1, int size = 30, string vid = null,
                                                                    string author = null, int authorId = 0,
                                                                    string startDate = null,
                                                                    string endDate = null, int mode = 10,
                                                                    string ip = null, string key = null,
                                                                    bool descending = true)
        {
            var result = await DanmuDao.DanmuBasesSelect(page, size, vid, author, authorId, startDate, endDate, mode,
                    ip,
                    key, descending);
            return new WebResult<DanmuTable[]>(result);
        }
    }
}
