using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Base
{
	[ApiController]
	public class DanmakuDaoBaseApiController : ControllerBase
	{
		private protected readonly IDanmakuDao Dao;

		public DanmakuDaoBaseApiController(IDanmakuDao danmakuDao)
		{
			Dao = danmakuDao;
		}
	}
}