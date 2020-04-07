using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Danmu.Model.Config;
using Danmu.Model.Danmu.BiliBili;
using Danmu.Utils.Configuration;
using Danmu.Utils.Dao;

namespace Danmu.Utils.BiliBili
{
    public partial class BiliBiliHelp
    {
        private readonly CacheDao _cache;
        private readonly bool _canGetHistory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BiliBiliSetting _setting;

        public BiliBiliHelp(AppConfiguration appConfiguration, IHttpClientFactory httpClientFactory,
                            CacheDao biliBiliCache)
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
        public async Task<Stream> GetDanmuRawByCidAsync(int cid)
        {
            var r = await GetDanmuRawAsync($"https://api.bilibili.com/x/v1/dm/list.so?oid={cid}");
            return r.Length == 0 ? Stream.Null : new MemoryStream(r);
        }

        /// <summary>
        ///     通过Query获取弹幕
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Stream> GetDanmuRawByQueryAsync(BiliBiliQuery query)
        {
            query = await CheckCid(query);
            return await GetDanmuRawByCidAsync(query.Cid);
        }

        /// <summary>
        ///     获取B站历史弹幕
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <param name="date">历史日期</param>
        /// <returns></returns>
        public async Task<DanmuDataBiliBili> GetDanmuAsync(int cid, string[] date)
        {
            if (!_canGetHistory) return await GetDanmuAsync(new BiliBiliQuery {Cid = cid});
            var a = Task.Run(() => date.Select(async s =>
            {
                var b = await GetDanmuRawAsync(
                        $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}", true);
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
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DanmuDataBiliBili> GetDanmuAsync(BiliBiliQuery query)
        {
            query = await CheckCid(query);

            return query.Cid == 0
                    ? new DanmuDataBiliBili()
                    : query.Date.Length == 0 && !_canGetHistory
                            ? new DanmuDataBiliBili(await GetDanmuRawByCidAsync(query.Cid))
                            : await GetDanmuAsync(query.Cid, query.Date);
        }

        /// <summary>
        ///     检查Cid
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BiliBiliQuery> CheckCid(BiliBiliQuery query)
        {
            if (query.Cid == 0)
            {
                query.P = query.P == 0 ? 1 : query.P;
                if (query.Aid != 0)
                    query.Cid = await GetCidAsync(query.Aid, query.P);
                else if (!string.IsNullOrEmpty(query.Bvid)) query.Cid = await GetCidAsync(query.Bvid, query.P);
            }

            return query;
        }

        /// <summary>
        ///     获取视频aid和bvid信息
        /// </summary>
        /// <param name="bvid"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        public async Task<BvidInfo> GetBvidInfoAsync(string bvid, int aid)
        {
            string url;
            if (string.IsNullOrEmpty(bvid))
                url = $"https://api.bilibili.com/x/web-interface/archive/stat?aid={aid}";
            else if (aid == 0)
                url = $"https://api.bilibili.com/x/web-interface/archive/stat?bvid={bvid}";
            else
                return new BvidInfo
                {
                    Code = 0,
                    Data = new BvidInfo.DataObj
                    {
                        Aid = aid,
                        Bvid = bvid
                    }
                };
            var a = await GetBiliBiliPageRawAsync(url);
            if (a.Length > 0)
            {
                var b = await JsonSerializer.DeserializeAsync<BvidInfo>(new MemoryStream(a));
                if (b.Code == 0) return b;
            }

            return new BvidInfo();
        }
    }
}
