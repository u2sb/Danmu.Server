using System;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Danmu.Utils.Dao
{
    public class CacheDao
    {
        private readonly IDistributedCache _cache;

        public CacheDao(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        ///     HttpCache表中获取或创建
        /// </summary>
        /// <param name="key"></param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="factory"></param>
        /// <param name="slidingExpiration">过期时间</param>
        /// <returns></returns>
        public async Task<T> GetOrCreateCacheAsync<T>(string key, TimeSpan slidingExpiration,
                                                      TimeSpan absoluteExpiration, Func<Task<T>> factory)
        {
            T t = default;
            var a = await _cache.GetAsync(key);
            if (a == null)
            {
                t = await factory.Invoke();
                if (t != null)
                    await _cache.SetAsync(key, MessagePackSerializer.Serialize(t), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = absoluteExpiration,
                        SlidingExpiration = slidingExpiration
                    });
            }
            else
            {
                t = MessagePackSerializer.Deserialize<T>(a);
            }

            return t;
        }

        /// <summary>
        ///     清空Http缓存表
        /// </summary>
        public Task ClearCacheAsync(string key)
        {
            return _cache.RefreshAsync(key);
        }
    }
}
