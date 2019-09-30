using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Dplayer.DanmakuList
{
	[Route("dplayer/danmakulist")]
	[Authorize]
	public class DanmakuListController : Controller
	{
		private readonly IDanmakuDao _dao;

		public DanmakuListController(IDanmakuDao dao)
		{
			_dao = dao;
		}


		public ActionResult Index()
		{
			return View(_dao.DanmakuBaseQuery(1, 10));
		}

//		[HttpPost]
//        public ActionResult Index(int page, int size = 10)
//        {
//	        return View(_dao.DanmakuBaseQuery(page, size));
//		}

		[HttpPost]
		public ActionResult Index(string vid)
		{
			return View(_dao.DanmakuBasesQueryByVid(vid, 1, 10));
		}
	}
}