using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.Danmu.DanmuData;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Danmu.Dplayer.V3
{
    [Route("api/danmu/dplayer/v3")]
    public class DplayerController : DanmuBaseController
    {
        public DplayerController(DanmuDao danmuDao, VideoDao videoDao) : base(danmuDao, videoDao) { }

        // GET: api/dplayer/v3/
        [HttpGet]
        public async Task<DplayerWebResult> Get(string id)
        {
            id ??= Request.Query["id"];
            return string.IsNullOrEmpty(id)
                    ? new DplayerWebResult(1)
                    : new DplayerWebResult(await DanmuDao.QueryByVidAsync(id));
        }

        [HttpPost]
        public async Task<WebResult> Post([FromBody] DplayerDanmuDataIn data)
        {
            if (string.IsNullOrWhiteSpace(data.Id) || string.IsNullOrWhiteSpace(data.Text))
                return new WebResult(1);
            data.Ip = IPAddress.TryParse(Request.Headers["X-Real-IP"], out var ip)
                    ? ip
                    : Request.HttpContext.Connection.RemoteIpAddress;
            data.Referer = Request.Headers["Referer"].FirstOrDefault();

            var danmu = new DanmuTable
            {
                Vid = data.Id,
                Data = data.ToBaseDanmuData(),
                Ip = data.Ip
            };
            var result = await DanmuDao.InsertAsync(danmu);
            await VideoDao.InsertAsync(data.Id, new Uri(data.Referer));
            return new WebResult(result ? 0 : 1);
        }
    }
}
