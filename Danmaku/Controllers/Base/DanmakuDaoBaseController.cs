using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Danmaku.Controllers.Base
{
	[ApiController]
	public class DanmakuDaoBaseController : Controller
	{
		private protected readonly IDanmakuDao Dao;
		private protected readonly ILogger<DanmakuDaoBaseController> Logger;

		public DanmakuDaoBaseController(IDanmakuDao danmakuDao, ILogger<DanmakuDaoBaseController> logger)
		{
			Dao = danmakuDao;
			Logger = logger;
		}
	}
}