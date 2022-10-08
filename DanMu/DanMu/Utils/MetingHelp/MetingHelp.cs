using System.Text.Json;
using DanMu.Models.Meting;
using DanMu.Models.Settings;
using DanMu.Utils.Cache;
using Meting4Net.Core;
using MMeting = Meting4Net.Core.Meting;
using static DanMu.Models.Meting.QueryType;

namespace DanMu.Utils.MetingHelp;

public class MetingHelp
{
    private readonly MetingCache _caching;
    private readonly AppSettings _config;
    private readonly MMeting _meting;

    public MetingHelp(AppSettings appSettings, MetingCache cache)
    {
        _config = appSettings;
        _meting = new MMeting(appSettings.Meting.DefaultServerProvider);
        _caching = cache;
    }

    public MetingHelp SetServer(ServerProvider server)
    {
        _meting.Server = server;
        return this;
    }

    public async Task<string?> GetLrcAsync(string id)
    {
        return await _caching.GetAsync($"{nameof(GetLrcAsync)}.{id}", async () =>
            {
                return await Task.Run(() =>
                {
                    var lrc = _meting.LyricObj(id);
                    return lrc.lyric;
                });
            },
            TimeSpan.FromMinutes(_config.Meting.CachingTime.Lrc));
    }

    public async Task<string?> GetPicAsync(string id)
    {
        return await _caching.GetAsync($"{nameof(GetPicAsync)}.{id}", async () =>
            {
                return await Task.Run(() =>
                {
                    var pic = _meting.PicObj(id, 90);
                    var picUrl = pic.url;
                    if (!string.IsNullOrEmpty(picUrl))
                        picUrl = Replace(picUrl, _config.Meting.Replace.Pic);
                    return picUrl;
                });
            },
            TimeSpan.FromMinutes(_config.Meting.CachingTime.Pic));
    }

    public async Task<string?> GetUrlAsync(string id)
    {
        return await _caching.GetAsync($"{nameof(GetUrlAsync)}.{id}", async () =>
            {
                return await Task.Run(() =>
                {
                    var url = _meting.UrlObj(id);
                    var urlUrl = url.url;
                    if (!string.IsNullOrEmpty(urlUrl))
                        urlUrl = Replace(urlUrl, _config.Meting.Replace.Url);
                    return urlUrl;
                });
            },
            TimeSpan.FromMinutes(_config.Meting.CachingTime.Url));
    }

    public async Task<string?> SearchAsync(QueryModel query, string baseUrl)
    {
        var list = await _caching.GetAsync($"{nameof(SearchAsync)}.{query.Id}", async () =>
            {
                return await Task.Run(() =>
                {
                    MusicList[] musicList = { };
                    switch (query.Type)
                    {
                        case Album:
                            musicList = _meting.AlbumObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl))
                                .ToArray();
                            break;
                        case Artist:
                            musicList = _meting.ArtistObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl))
                                .ToArray();
                            break;
                        case PlayList:
                            musicList = _meting.PlaylistObj(query.Id)
                                .Select(s => new MusicList(s, query.Server, baseUrl)).ToArray();
                            break;
                        case Search:
                            musicList = _meting.SearchObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl))
                                .ToArray();
                            break;
                        case Song:
                            musicList = new MusicList[] { new(_meting.SongObj(query.Id), query.Server, baseUrl) };
                            break;
                    }

                    return musicList;
                });
            },
            TimeSpan.FromMinutes(_config.Meting.CachingTime.Base));

        return JsonSerializer.Serialize(list);
    }

    private string Replace(string url, List<List<string>>? p)
    {
        if (p != null)
            foreach (var v in p)
                url = url.Replace(v[0], v[1]);
        return url;
    }
}