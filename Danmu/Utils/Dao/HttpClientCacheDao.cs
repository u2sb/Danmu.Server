using System;
using System.Linq;
using System.Threading.Tasks;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Utils.Dao
{
    public class HttpClientCacheDao
    {
        private readonly DanmuContext _con;

        public HttpClientCacheDao(DanmuContext con)
        {
            _con = con;
        }

        /// <summary>
        ///     获取或创建
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireTime"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<byte[]> GetOrCreateCache(string key, TimeSpan expireTime, Func<string, Task<byte[]>> factory)
        {
            var a = _con.HttpClientCache.Where(e => e.Data.Key.Equals(key));
            if (await a.CountAsync() > 0)
            {
                var b = await a.FirstOrDefaultAsync();
                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - b.Data.TimeStamp > expireTime.TotalSeconds)
                {
                    var c = await factory(key);
                    b.Data.Value = c;
                    b.Data.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    _con.HttpClientCache.Update(b);
                    await _con.SaveChangesAsync();
                }

                return b.Data.Value;
            }

            var d = new HttpClientCacheTable
            {
                Data = new CacheData
                {
                    Key = key,
                    Value = await factory(key),
                    TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                }
            };
            await _con.HttpClientCache.AddAsync(d);
            await _con.SaveChangesAsync();
            return d.Data.Value;
        }
    }
}
