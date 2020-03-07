using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Controllers.Base
{
    [ApiController]
    [EnableCors(AdminAllowSpecificOrigins)]
    [Authorize(Policy = AdminRolePolicy)]
    public class AdminBaseController : ControllerBase
    {
        private protected DanmuDao DanmuDao;
        private protected VideoDao VideoDao;

        protected AdminBaseController(DanmuDao danmuDao, VideoDao videoDao)
        {
            DanmuDao = danmuDao;
            VideoDao = videoDao;
        }
    }
}
