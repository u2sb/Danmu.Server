using Danmu.Utils.Dao.Danmu;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Danmu.Utils.Globals.VariableDictionary;

namespace Danmu.Controllers.Base
{
    [ApiController]
    [EnableCors(DanmuAllowSpecificOrigins)]
    [FormatFilter]
    public abstract class DanmuBaseController : ControllerBase
    {
        internal DanmuDao DanmuDao;
        protected DanmuBaseController(DanmuDao danmu)
        {
            DanmuDao = danmu;
        }
    }
}
