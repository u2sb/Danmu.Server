using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Models.Danmus.BiliBili;
using Danmu.Models.Danmus.Danmu;
using Danmu.Models.WebResults;
using Danmu.Utils.Dao.Danmu;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Danmu.Controllers.Danmu.Common.V1
{
    [Route("/api/danmu/v1")]
    public class CommonController : DanmuBaseController
    {
        public CommonController(DanmuDao danmu, VideoDao video) : base(danmu, video)
        {
        }


        [HttpGet]
        [HttpGet("{id}.{format?}")]
        public async Task<dynamic> Get(string id, string format)
        {
            id ??= Request.Query["id"];
            if (string.IsNullOrEmpty(id)) return new WebResult(1);

            var result = await DanmuDao.QueryDanmusByVidAsync(id);

            if (!string.IsNullOrEmpty(format) && format.Equals("xml")) return (BiliBiliDanmuData) result;

            return new WebResult<BaseDanmuData[]>(result);
        }
    }
}
