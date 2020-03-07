using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.Danmu.BiliBili;
using Danmu.Model.Danmu.DanmuData;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.Common.V1
{
    [Route("/api/danmu/v1")]
    [FormatFilter]
    public class CommonController : DanmuBaseController
    {
        public CommonController(DanmuDao danmuDao, VideoDao videoDao) : base(danmuDao, videoDao) { }

        [HttpGet]
        [HttpGet("{id}.{format?}")]
        public async Task<dynamic> Get(string id, string format)
        {
            id ??= Request.Query["id"];
            if (string.IsNullOrEmpty(id)) return new WebResult(1);

            var result = await DanmuDao.QueryDanmusByVidAsync(id);

            if (!string.IsNullOrEmpty(format) && format.Equals("xml")) return (DanmuDataBiliBili) result;

            return new WebResult<BaseDanmuData[]>(result);
        }

        [HttpPost]
        public async Task<WebResult> Post([FromBody] BaseDanmuDataIn data)
        {
            if (string.IsNullOrWhiteSpace(data.Id) || string.IsNullOrWhiteSpace(data.Text))
                return new WebResult(1);
            data.Ip = IPAddress.TryParse(Request.Headers["X-Real-IP"], out var ip)
                    ? ip
                    : Request.HttpContext.Connection.RemoteIpAddress;
            data.Referer ??= Request.Headers["Referer"].FirstOrDefault();
            var video = await VideoDao.InsertAsync(data.Id, new Uri(data.Referer));
            var danmu = new DanmuTable
            {
                Vid = data.Id,
                Data = data,
                Ip = data.Ip,
                Video = video
            };
            var result = await DanmuDao.InsertDanmuAsync(danmu);
            return new WebResult(result ? 0 : 1);
        }
    }
}
