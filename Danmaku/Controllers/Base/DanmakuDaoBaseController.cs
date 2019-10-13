using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Base
{
	[ApiController]
	public class DanmakuDaoBaseController : Controller
	{
		private protected readonly IDanmakuDao Dao;

		public DanmakuDaoBaseController(IDanmakuDao danmakuDao)
		{
			Dao = danmakuDao;
		}
	}
}