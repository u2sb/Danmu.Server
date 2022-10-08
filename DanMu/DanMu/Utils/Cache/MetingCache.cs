using EasyCaching.Core;
using static DanMu.Models.Settings.ConstantTable;

namespace DanMu.Utils.Cache;

public class MetingCache
{
    private readonly IEasyCachingProvider _caching;

    public MetingCache(IEasyCachingProviderFactory caching)
    {
        _caching = caching.GetCachingProvider(MetingCacheLiteDbName);
    }

    /// <summary>
    ///     获取或刷新缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <param name="expiration"></param>
    /// <returns></returns>
    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
    {
        T? t;
        var a = await _caching.GetAsync<T>(key);
        if (!a.HasValue || a.IsNull)
        {
            t = await factory.Invoke();
            if (t != null) await _caching.SetAsync(key, t, expiration);
        }
        else
        {
            t = a.Value;
        }

        return t;
    }
}