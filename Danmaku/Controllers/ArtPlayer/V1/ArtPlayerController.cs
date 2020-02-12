using System.Net.Http;
using System.Threading.Tasks;
using Danmaku.Controllers.Base;
using Danmaku.Model;
using Danmaku.Utils.Dao;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.ArtPlayer.V1
{
    [Route("api/artplayer/v1")]
    [ApiController]
    public class ArtPlayerController : DanmakuDaoBaseApiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArtPlayerController(IDanmakuDao danmakuDao, IHttpClientFactory httpClientFactory) : base(danmakuDao)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<string> Get(string id)
        {
            return string.IsNullOrEmpty(id) ? null : new BilibiliDanmakuData(await Dao.DanmakuQuery(id)).ToXml();
        }
    }
}