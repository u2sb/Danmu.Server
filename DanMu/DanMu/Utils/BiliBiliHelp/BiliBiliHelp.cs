using Bilibili.Community.Service.Dm.V1;
using DanMu.Models.BiliBili;
using DanMu.Models.DanMu.Generic.V1;
using DanMu.Models.Settings;
using DanMu.Utils.Cache;
using Google.Protobuf;
using RestSharp;

namespace DanMu.Utils.BiliBiliHelp;

public partial class BiliBiliHelp
{
    // 接口
    private const string BaseUrl = "https://api.bilibili.com";
    private const string PageUrl = "/x/player/pagelist";
    private const string DanMuUrl = "/x/v2/dm/list/seg.so";


    private readonly BiliBiliCache _caching;
    private readonly RestClient _client;
    private readonly BiliBiliSetting _setting;

    public BiliBiliHelp(BiliBiliCache caching, AppSettings setting, RestClient client)
    {
        _caching = caching;
        _setting = setting.BiliBiliSetting;
        _client = client;
    }

    /// <summary>
    ///     获取B站弹幕 并返回通用弹幕格式
    /// </summary>
    /// <param name="query"></param>
    /// <param name="id"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public async Task<GenericDanMu[]?> GetGenericDanMuAsync(BQuery query, string? id, int p)
    {
        if (!string.IsNullOrWhiteSpace(id) && id.StartsWith("BV"))
        {
            query.BVid = id;
            query.P = p;
        }
        else if (!string.IsNullOrWhiteSpace(id) && id.StartsWith("av"))
        {
            if (int.TryParse(id.Remove(0, 2), out var aid))
            {
                query.Aid = aid;
                query.P = p;
            }
        }

        return await GetGenericDanMuAsync(query);
    }

    /// <summary>
    ///     获取B站弹幕 并返回通用弹幕格式
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<GenericDanMu[]?> GetGenericDanMuAsync(BQuery query)
    {
        var cidAndDuration = await (query.Aid == 0
            ? GetCidAndDurationAsync(query.BVid, query.P)
            : GetCidAndDurationAsync(query.Aid, query.P));


        if (cidAndDuration != null && cidAndDuration.Cid != 0)
            return await _caching.GetAsync($"{nameof(GetGenericDanMuAsync)}{cidAndDuration.Cid}",
                async () =>
                {
                    var ds = await GetDanMuNoCacheAsync(query, cidAndDuration);
                    return ds?.Elems.Select(s => (GenericDanMu)s).ToArray();
                },
                TimeSpan.FromHours(_setting.DanMuCacheTime));

        return Array.Empty<GenericDanMu>();
    }

    /// <summary>
    ///     获取B站弹幕
    /// </summary>
    /// <param name="query"></param>
    /// <param name="id"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public async Task<DmSegMobileReply?> GetDanMuAsync(BQuery query, string? id, int p)
    {
        if (!string.IsNullOrWhiteSpace(id) && id.StartsWith("BV"))
        {
            query.BVid = id;
            query.P = p;
        }
        else if (!string.IsNullOrWhiteSpace(id) && id.StartsWith("av"))
        {
            if (int.TryParse(id.Remove(0, 2), out var aid))
            {
                query.Aid = aid;
                query.P = p;
            }
        }

        return await GetDanMuAsync(query);
    }


    /// <summary>
    ///     获取B站弹幕
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<DmSegMobileReply?> GetDanMuAsync(BQuery query)
    {
        var cidAndDuration = await (query.Aid == 0
            ? GetCidAndDurationAsync(query.BVid, query.P)
            : GetCidAndDurationAsync(query.Aid, query.P));


        if (cidAndDuration != null && cidAndDuration.Cid != 0)
            return await _caching.GetAsync($"{nameof(GetDanMuAsync)}{cidAndDuration.Cid}",
                async () => await GetDanMuNoCacheAsync(query, cidAndDuration),
                TimeSpan.FromHours(_setting.DanMuCacheTime));

        return null;
    }


    /// <summary>
    ///     请求原始数据
    /// </summary>
    /// <param name="path"></param>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    private async Task<byte[]?> GetBiliBiliDataRawAsync(string path, Dictionary<string, string>? queryParams)
    {
        var request = new RestRequest(BaseUrl + path);
        if (queryParams != null)
            foreach (var item in queryParams)
                request.AddQueryParameter(item.Key, item.Value, false);

        return await _client.DownloadDataAsync(request);
    }

    /// <summary>
    ///     格式化数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="parser"></param>
    /// <returns></returns>
    private T Parse<T>(byte[] data, MessageParser<T> parser) where T : IMessage<T>
    {
        return parser.ParseFrom(data);
    }

    /// <summary>
    ///     获取B站弹幕无缓存
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cidAndDuration"></param>
    /// <returns></returns>
    private async Task<DmSegMobileReply?> GetDanMuNoCacheAsync(BQuery query, CidAndDuration cidAndDuration)
    {
        var d = cidAndDuration.Duration / 360 + 1;
        var getDanMuTaskList = new List<Task<byte[]>>();

        for (var i = 0; i < d; i++)
        {
            var a = GetBiliBiliDataRawAsync(DanMuUrl, new Dictionary<string, string>
            {
                { "type", query.Type.ToString() },
                { "oid", cidAndDuration.Cid.ToString() },
                { "segment_index", (i + 1).ToString() }
            });

            getDanMuTaskList.Add(a!);
        }

        var danMuRawList = await Task.WhenAll(getDanMuTaskList);

        var danMuResponseList = danMuRawList.Where(w => !Equals(w, Stream.Null)).Select(s => s);


        var danMuSegList = danMuResponseList.Select(s => Parse(s, DmSegMobileReply.Parser));

        var dmSeg = new DmSegMobileReply
        {
            State = 0
        };

        foreach (var danMuSeg in danMuSegList) dmSeg.Elems.Add(danMuSeg.Elems);

        if (dmSeg.Elems.Count > 0) return dmSeg;

        return null;
    }
}