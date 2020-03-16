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
        ///     获取弹幕
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<DanmuListWebResult<DanmuTable>> GetDanmuList(string vid = null, int page = 1, int size = 30,
                                                                       bool descending = true)
        {
            var total = string.IsNullOrEmpty(vid)
                    ? DanmuDao.GetAllDanmuAsync()
                    : DanmuDao.GetDanmuByVidAsync(vid);

            var danmu = string.IsNullOrEmpty(vid)
                    ? DanmuDao.GetAllDanmuAsync(page, size, descending)
                    : DanmuDao.GetDanmuByVidAsync(vid, page, size, descending);

            return new DanmuListWebResult<DanmuTable>(await total, await danmu);
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
        public async Task<DanmuListWebResult<DanmuTable>> DateSelect(int page = 1, int size = 30,
                                                                     string startDate = null,
                                                                     string endDate = null, bool descending = true)
        {
            var result = DanmuDao.DateSelectAsync(page, size, startDate, endDate);
            return new DanmuListWebResult<DanmuTable>(0)
            {
                Data = await result
            };
        }

        /// <summary>
        ///     基础查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("base" + "select")]
        public async Task<DanmuListWebResult<DanmuTable>> DanmuBasesSelect(
                int page = 1, int size = 30, string vid = null,
                string author = null, string authorId = null,
                string startDate = null,
                string endDate = null, int mode = 100,
                string ip = null, string key = null,
                bool descending = true)
        {
            var iAuthorId = int.TryParse(authorId, out var uid) ? uid : -1;
            var result = DanmuDao.DanmuBasesSelectAsync(page, size, vid, author, iAuthorId, startDate, endDate,
                    mode,
                    ip,
                    key, descending);
            return new DanmuListWebResult<DanmuTable>(0)
            {
                Data = await result
            };
        }
    }
}
