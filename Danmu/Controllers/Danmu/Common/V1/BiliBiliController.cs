using System.Collections.Generic;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.Danmu.DanmuData;
using Danmu.Model.WebResult;
using Danmu.Utils.BiliBili;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.Common.V1
{
    [Route("/api/danmu/v1/bilibili")]
    public class BiliBiliController : BiliBiliBaseController
    {
        public BiliBiliController(BiliBiliHelp bilibili) : base(bilibili) { }

        [HttpGet]
        [HttpGet("danmu")]
        [HttpGet("danmu.{format}")]
        public async Task<dynamic> Get(int cid, int aid, string bvid, int p, string format)
        {
            string[] date = Request.Query["date"];
            if (date.Length == 0 && !(!string.IsNullOrEmpty(format) && format.Equals("json")))
            {
                if (cid == 0)
                {
                    if (aid != 0)
                    {
                        p = p == 0 ? 1 : p;
                        cid = await Bilibili.GetCidAsync(aid, p);
                    }
                    else if (!string.IsNullOrEmpty(bvid))
                    {
                        p = p == 0 ? 1 : p;
                        cid = await Bilibili.GetCidAsync(bvid, p);
                    }
                }

                HttpContext.Response.ContentType = "application/xml; charset=utf-8";
                return await Bilibili.GetDanmuRawByCidTaskAsync(cid);
            }

            var danmu = await Bilibili.GetDanmuAsync(cid, aid, bvid, p, date);

            if (!string.IsNullOrEmpty(format) && format.Equals("json"))
                return new WebResult<IEnumerable<BaseDanmuData>>(danmu.ToDanmuDataBases());

            if (string.IsNullOrEmpty(format)) HttpContext.Request.Headers["Accept"] = "application/xml";
            return danmu;
        }
    }
}
