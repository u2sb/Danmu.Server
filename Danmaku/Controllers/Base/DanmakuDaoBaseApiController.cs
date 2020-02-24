using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Base
{
    [ApiController]
    public class DanmakuDaoBaseApiController : ControllerBase
    {
        private protected readonly DanmakuDao Dao;

        public DanmakuDaoBaseApiController(DanmakuDao danmakuDao)
        {
            Dao = danmakuDao;
        }
    }
}