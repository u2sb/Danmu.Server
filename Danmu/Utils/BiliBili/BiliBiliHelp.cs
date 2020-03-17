using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Danmu.Model.Config;
using Danmu.Model.Danmu.BiliBili;
using Danmu.Utils.Configuration;
using Danmu.Utils.Dao;
using Microsoft.Extensions.Caching.Memory;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Utils.BiliBili
{
    public partial class BiliBiliHelp
    {
        private readonly bool _canGetHistory;
        private readonly HttpClient _deflateClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientCacheDao _cache;
        private readonly BiliBiliSetting _setting;

        public BiliBiliHelp(AppConfiguration appConfiguration, IHttpClientFactory httpClientFactory, HttpClientCacheDao biliBiliCache)
        {
            _httpClientFactory = httpClientFactory;
            _deflateClient = httpClientFactory.CreateClient(Deflate);
            _setting = appConfiguration.GetAppSetting().BiliBiliSetting;
            if (!string.IsNullOrEmpty(_setting.Cookie))
            {
                _canGetHistory = true;
                _deflateClient.DefaultRequestHeaders.Add("Cookie", _setting.Cookie);
            }

            _cache = biliBiliCache;
        }

        /// <summary>
        ///     获取B站弹幕
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <returns>B站弹幕</returns>
        public async Task<DanmuDataBiliBili> GetDanmuAsync(int cid)
        {
            var raw = GetDanmuRawByCidTask(cid);
            return new DanmuDataBiliBili(await raw);
        }

        /// <summary>
        ///     获取B站历史弹幕
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <param name="date">历史日期</param>
        /// <returns></returns>
        public async Task<DanmuDataBiliBili> GetDanmuAsync(int cid, string[] date)
        {
            if (!_canGetHistory) return new DanmuDataBiliBili();
            var a = Task.Run(() => date.Select(async s =>
            {
                var b = await GetDanmuRawAsync(
                        $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}");
                return new DanmuDataBiliBili(b);
            }).SelectMany(s => s.Result.D));
            return new DanmuDataBiliBili
            {
                D = (await a).ToArray()
            };
        }

        /// <summary>
        ///     获取B站弹幕
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="aid"></param>
        /// <param name="p"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<DanmuDataBiliBili> GetDanmuAsync(int cid, int aid, int p, string[] date)
        {
            if (cid == 0 && aid != 0)
            {
                p = p == 0 ? 1 : p;
                cid = await GetCidAsync(aid, p);
            }

            return cid == 0
                    ? new DanmuDataBiliBili()
                    : date.Length == 0
                            ? await GetDanmuAsync(cid)
                            : await GetDanmuAsync(cid, date);
        }
    }
}
