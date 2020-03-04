using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Controllers.Admin
{
    [Authorize(Policy = AdminRolePolicy)]
    [ApiController]
    [Route("/admin/danmuList")]
    public class DanmuListController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "11111";
        }
    }
}