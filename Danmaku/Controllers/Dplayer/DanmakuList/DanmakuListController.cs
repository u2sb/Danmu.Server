using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Dplayer.DanmakuList
{
	[Route("dplayer/danmakulist")]
	[Authorize]
	public class DanmakuListController : DanmakuDaoBaseController
	{
		public DanmakuListController(IDanmakuDao danmakuDao) : base(danmakuDao)
		{
		}


		public async Task<ActionResult> Index(int page = 1, int size = 10)
		{
			return View(await Dao.DanmakuBaseQuery(page, size));
		}

		[HttpGet("select")]
		public ActionResult Select()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Index([FromForm] string vid, [FromForm] int page = 1, [FromForm] int size = 10)
		{
			return View(await Dao.DanmakuBasesQueryByVid(vid, page, size));
		}

		[HttpGet("editor/{id}")]
		public async Task<ActionResult> Editor(string id)
		{
			var data = await Dao.DanmakuBaseQuery(id);
			if (data == null)
				return BadRequest();
			return View(data);
		}
	}
}