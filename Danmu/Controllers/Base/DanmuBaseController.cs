using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Controllers.Base
{
    [ApiController]
    [EnableCors(DanmuAllowSpecificOrigins)]
    [FormatFilter]
    public abstract class DanmuBaseController : ControllerBase
    {
        private protected DanmuDao DanmuDao;
        private protected VideoDao VideoDao;

        protected DanmuBaseController(DanmuDao danmuDao, VideoDao videoDao)
        {
            DanmuDao = danmuDao;
            VideoDao = videoDao;
        }
    }
}
