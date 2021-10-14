using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Models.Danmus.BiliBili;
using Danmu.Models.Danmus.Danmu;
using Danmu.Models.Danmus.DataTables;
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


        [HttpPost]
        public async Task<WebResult> Post([FromBody] BaseDanmuDataIn data)
        {
            if (string.IsNullOrWhiteSpace(data.Id) || string.IsNullOrWhiteSpace(data.Text))
                return new WebResult(1);
            data.Ip = IPAddress.TryParse(Request.Headers["X-Real-IP"], out var ip)
                ? ip
                : Request.HttpContext.Connection.RemoteIpAddress;
            data.Referer ??= Request.Headers["Referer"].FirstOrDefault();
            var video = VideoDao.Insert(data.Id, new Uri(data.Referer ?? string.Empty));
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
