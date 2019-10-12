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


		public ActionResult Index()
		{
			return View(Dao.DanmakuBaseQuery(1, 10));
		}

//		[HttpPost]
//        public ActionResult Index(int page, int size = 10)
//        {
//	        return View(Dao.DanmakuBaseQuery(page, size));
//		}

		[HttpPost]
		public ActionResult Index(string vid)
		{
			return View(Dao.DanmakuBasesQueryByVid(vid, 1, 10));
		}
	}
}