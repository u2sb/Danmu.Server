using Danmu.Utils.BiliBiliHelp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Danmu.Utils.Globals.VariableDictionary;

namespace Danmu.Controllers.Base
{
    [EnableCors(DanmuAllowSpecificOrigins)]
    [FormatFilter]
    [ApiController]
    public class BiliBiliBaseController : ControllerBase
    {
        private protected readonly BiliBiliHelp Bilibili;

        public BiliBiliBaseController(BiliBiliHelp bilibili)
        {
            Bilibili = bilibili;
        }
    }
}
