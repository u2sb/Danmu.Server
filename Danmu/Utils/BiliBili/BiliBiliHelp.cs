using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Danmu.Model.Config;
using Danmu.Model.Danmu.BiliBili;
using Danmu.Utils.Configuration;
using Danmu.Utils.Dao;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Utils.BiliBili
{
    public partial class BiliBiliHelp
    {
        private readonly bool _canGetHistory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientCacheDao _cache;
        private readonly BiliBiliSetting _setting;

        public BiliBiliHelp(AppConfiguration appConfiguration, IHttpClientFactory httpClientFactory, HttpClientCacheDao biliBiliCache)
        {
            _httpClientFactory = httpClientFactory;
            _setting = appConfiguration.GetAppSetting().BiliBiliSetting;
            _canGetHistory = !string.IsNullOrEmpty(_setting.Cookie);
            _cache = biliBiliCache;
        }

        /// <summary>
        ///     获取B站弹幕
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <returns>B站弹幕数据流</returns>
        public async Task<Stream> GetDanmuRawByCidTaskAsync(int cid)
        {
            var r = await GetDanmuRawAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}");
            return r.Length == 0 ? Stream.Null : new MemoryStream(r);
        }

        /// <summary>
        ///     获取B站历史弹幕
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <param name="date">历史日期</param>
        /// <returns></returns>
        public async Task<DanmuDataBiliBili> GetDanmuAsync(int cid, string[] date)
        {
            if (!_canGetHistory) return await GetDanmuAsync(cid, 0, 1, new string[0]);
            var a = Task.Run(() => date.Select(async s =>
            {
                var b = await GetDanmuRawAsync(
                        $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}");
                var c = b.Length == 0 ? Stream.Null : new MemoryStream(b);
                return new DanmuDataBiliBili(c);
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
                            ? new DanmuDataBiliBili(await GetDanmuRawByCidTaskAsync(cid))
                            : await GetDanmuAsync(cid, date);
        }
    }
}
