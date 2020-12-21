using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Models.Danmus.Danmu;
using Danmu.Models.Danmus.DataTables;
using Danmu.Models.WebResults;
using Danmu.Utils.Dao.Danmu;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.Dplayer.V3
{
    [Route("/api/danmu/dplayer/v3")]
    public class DplayerController : DanmuBaseController
    {
        public DplayerController(DanmuDao danmuDao, VideoDao videoDao) : base(danmuDao, videoDao)
        {
        }

        // GET: api/dplayer/v3/
        [HttpGet]
        public async Task<DplayerWebResult> Get(string id)
        {
            id ??= Request.Query["id"];
            return string.IsNullOrEmpty(id)
                ? new DplayerWebResult(1)
                : new DplayerWebResult(await DanmuDao.QueryDanmusByVidAsync(id));
        }

        [HttpPost]
        public async Task<WebResult> Post([FromBody] DplayerDanmuDataIn data)
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
                Data = data.ToBaseDanmuData(),
                Ip = data.Ip,
                Video = video
            };
            var result = await DanmuDao.InsertDanmuAsync(danmu);
            return new WebResult(result ? 0 : 1);
        }
    }
}
