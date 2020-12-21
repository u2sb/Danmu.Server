using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Danmu.Models.Configs;
using Danmu.Models.Danmus.BiliBili;
using Danmu.Models.Danmus.Danmu;
using Danmu.Utils.Caches;

namespace Danmu.Utils.BiliBiliHelp
{
    public partial class BiliBiliHelp
    {
        private readonly CacheLiteDb _caching;
        private readonly bool _canGetHistory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BiliBiliSetting _setting;

        public BiliBiliHelp(AppSettings appSettings, IHttpClientFactory clientFactory,
            CacheLiteDb cache)
        {
            _httpClientFactory = clientFactory;
            _caching = cache;
            _setting = appSettings.BiliBiliSetting;
            _canGetHistory = !string.IsNullOrEmpty(_setting.Cookie);
        }

        /// <summary>
        ///     获取B站历史弹幕
        /// </summary>
        /// <param name="cid">视频的cid</param>
        /// <param name="date">历史日期</param>
        /// <returns></returns>
        public async Task<BaseDanmuData[]> GetDanmuAsync(int cid, string[] date)
        {
            if (!_canGetHistory) return await GetDanmuAsync(new BiliBiliQuery {Cid = cid});

            var a = Task.Run(() => date.Select(async s =>
            {
                return await _caching.GetAsync(
                    $"{nameof(GetDanmuAsync)}{cid}{s}",
                    async () => new BiliBiliDanmuData(await GetDanmuRawAsync(
                        $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}", true)),
                    TimeSpan.FromHours(_setting.DanmuCacheTime));
            }).SelectMany(s => s.Result.D));
            return new BiliBiliDanmuData
            {
                D = (await a).ToArray()
            }.ToBaseDanmuDatas().ToArray();
        }

        /// <summary>
        ///     获取B站弹幕
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<BaseDanmuData[]> GetDanmuAsync(BiliBiliQuery query)
        {
            query = await CheckCid(query);

            return await _caching.GetAsync($"{nameof(GetDanmuAsync)}{query.Cid}",
                async () => query.Cid == 0
                    ? null
                    : _canGetHistory && query.Date.Length != 0
                        ? await GetDanmuAsync(query.Cid, query.Date)
                        : new BiliBiliDanmuData(await GetDanmuRawAsync(
                            $"https://api.bilibili.com/x/v1/dm/list.so?oid={query.Cid}")).ToBaseDanmuDatas().ToArray(),
                TimeSpan.FromHours(_setting.DanmuCacheTime));
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
            if (string.IsNullOrEmpty(bvid))
                return await _caching.GetAsync($"{nameof(GetBvidInfoAsync)}{aid}",
                    async () =>
                    {
                        var a = await GetBiliBiliPageRawAsync(
                            $"https://api.bilibili.com/x/web-interface/archive/stat?aid={aid}");
                        if (a != Stream.Null)
                        {
                            var b = await JsonSerializer.DeserializeAsync<BvidInfo>(a);
                            if (b.Code == 0) return b;
                        }

                        return null;
                    },
                    TimeSpan.FromHours(_setting.CidCacheTime));

            if (aid == 0)
                return await _caching.GetAsync($"{nameof(GetBvidInfoAsync)}{bvid}",
                    async () =>
                    {
                        var a = await GetBiliBiliPageRawAsync(
                            $"https://api.bilibili.com/x/web-interface/archive/stat?bvid={bvid}");
                        if (a != Stream.Null)
                        {
                            var b = await JsonSerializer.DeserializeAsync<BvidInfo>(a);
                            if (b != null && b.Code == 0) return b;
                        }

                        return null;
                    }, TimeSpan.FromHours(_setting.CidCacheTime));
            if (!string.IsNullOrEmpty(bvid))
                return new BvidInfo
                {
                    Code = 0,
                    Data = new BvidInfo.DataObj
                    {
                        Aid = aid,
                        Bvid = bvid
                    }
                };
            return new BvidInfo();
        }
    }
}
