using System;
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

        public BiliBiliHelp(AppConfiguration appConfiguration, IHttpClientFactory clientFactory,
                            CacheDao biliBiliCache)
        {
            _httpClientFactory = clientFactory;
            _setting = appConfiguration.GetAppSetting().BiliBiliSetting;
            _canGetHistory = !string.IsNullOrEmpty(_setting.Cookie);
            _cache = biliBiliCache;
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
                return await _cache.GetOrCreateCacheAsync($"{nameof(GetDanmuAsync)}{cid}{s}",
                        TimeSpan.FromHours(_setting.DanmuCacheTime), TimeSpan.FromHours(_setting.DanmuCacheTime * 4),
                        async () => new DanmuDataBiliBili(await GetDanmuRawAsync(
                                $"https://api.bilibili.com/x/v2/dm/history?type=1&oid={cid}&date={s}", true)));
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

            return await _cache.GetOrCreateCacheAsync($"{nameof(GetDanmuAsync)}{query.Cid}",
                    TimeSpan.FromHours(_setting.DanmuCacheTime), TimeSpan.FromHours(_setting.DanmuCacheTime * 4),
                    async () => query.Cid == 0
                            ? new DanmuDataBiliBili()
                            : _canGetHistory && query.Date.Length != 0
                                    ? await GetDanmuAsync(query.Cid, query.Date)
                                    : new DanmuDataBiliBili(await GetDanmuRawAsync(
                                            $"https://api.bilibili.com/x/v1/dm/list.so?oid={query.Cid}")));
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
                return await _cache.GetOrCreateCacheAsync($"{nameof(GetBvidInfoAsync)}{aid}",
                        TimeSpan.FromHours(_setting.CidCacheTime), TimeSpan.FromHours(_setting.CidCacheTime * 4),
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
                        });
            if (aid == 0)
                return await _cache.GetOrCreateCacheAsync($"{nameof(GetBvidInfoAsync)}{bvid}",
                        TimeSpan.FromHours(_setting.CidCacheTime), TimeSpan.FromHours(_setting.CidCacheTime * 4),
                        async () =>
                        {
                            var a = await GetBiliBiliPageRawAsync(
                                    $"https://api.bilibili.com/x/web-interface/archive/stat?bvid={bvid}");
                            if (a != Stream.Null)
                            {
                                var b = await JsonSerializer.DeserializeAsync<BvidInfo>(a);
                                if (b.Code == 0) return b;
                            }

                            return null;
                        });
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
