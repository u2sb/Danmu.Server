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

namespace Danmu.Controllers.Danmu.ArtPlayer.V1
{
    [Route("/api/danmu/artplayer/v1")]
    [FormatFilter]
    public class ArtPlayerController : DanmuBaseController
    {
        public ArtPlayerController(DanmuDao danmuDao, VideoDao videoDao) : base(danmuDao, videoDao) { }

        [HttpGet]
        [HttpGet("{id}.{format?}")]
        public async Task<dynamic> Get(string id, string format)
        {
            id ??= Request.Query["id"];
            if (string.IsNullOrEmpty(id)) return new WebResult(1);

            var result = await DanmuDao.QueryDanmusByVidAsync(id);

            if (!string.IsNullOrEmpty(format) && format.Equals("json"))
            {
                var aData = result.Select(s => (ArtPlayerDanmuData) s).ToArray();
                return new WebResult<ArtPlayerDanmuData[]>(aData);
            }

            if (string.IsNullOrEmpty(format)) HttpContext.Request.Headers["Accept"] = "application/xml";

            return (DanmuDataBiliBili) result;
        }

        [HttpPost]
        public async Task<WebResult> Post([FromBody] ArtPlayerDanmuDataIn data)
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
                Data = data.ToBaseDanmuData(),
                Ip = data.Ip,
                Video = video
            };
            var result = await DanmuDao.InsertDanmuAsync(danmu);
            return new WebResult(result ? 0 : 1);
        }
    }
}
