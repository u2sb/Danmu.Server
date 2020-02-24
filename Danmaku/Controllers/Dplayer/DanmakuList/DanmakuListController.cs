using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Danmaku.Controllers.Dplayer.DanmakuList
{
    [Route("dplayer/danmakulist")]
    [Authorize]
    public class DanmakuListController : DanmakuDaoBaseController
    {
        public DanmakuListController(DanmakuDao danmakuDao, ILogger<DanmakuListController> logger) : base(danmakuDao,
                logger) { }

        public async Task<ActionResult> Index(int page = 1, int size = 20, string vid = null, string author = null,
                                              string date_star = null, string date_end = null, int type = 3,
                                              string ip = null, string text = null,
                                              int order = 0)
        {
            if (type == 3) return View(await Dao.DanmakuBaseQuery(page, size));
            return View(
                    await Dao.DanmakuBasesQuery(page, size, vid, author, date_star, date_end, type, ip, text, order));
        }

        /// <summary>
        ///     编辑框
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("editor/{id}")]
        public async Task<ActionResult> Editor(string id)
        {
            var data = await Dao.DanmakuBaseQuery(id);
            if (data == null)
                return BadRequest();
            return View(data);
        }

        /// <summary>
        ///     修改弹幕
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost("editor/")]
        public async Task<ActionResult> Editor([FromForm] string id, [FromForm] float time, [FromForm] int type,
                                               [FromForm] string color, [FromForm] string text)
        {
            var data = await Dao.DanmakuEdit(id, time, type, color, text);
            if (data == null)
                return BadRequest();
            return View(data);
        }

        /// <summary>
        ///     删除弹幕
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete/{id}")]
        public async Task<bool> Delete(string id)
        {
            return await Dao.DanmakuDelete(id);
        }
    }
}