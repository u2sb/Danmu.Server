using System;
using System.Linq;
using System.Threading.Tasks;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Utils.Dao
{
    public class CacheDao
    {
        private readonly DanmuContext _con;

        public CacheDao(DanmuContext con)
        {
            _con = con;
        }

        /// <summary>
        ///     HttpCache表中获取或创建
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireTime"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<byte[]> GetOrCreateHttpCacheAsync(string key, TimeSpan expireTime, Func<Task<byte[]>> factory)
        {
            var a = _con.HttpClientCache.Where(e => e.Key.Equals(key));
            if (await a.CountAsync() > 0)
            {
                var b = await a.FirstOrDefaultAsync();
                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - b.TimeStamp > expireTime.TotalSeconds)
                {
                    var c = await factory();
                    b.Value = c;
                    b.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    _con.HttpClientCache.Update(b);
                    await _con.SaveChangesAsync();
                }

                return b.Value;
            }

            var d = new HttpClientCacheTable
            {
                Key = key,
                Value = await factory()
            };
            await _con.HttpClientCache.AddAsync(d);
            await _con.SaveChangesAsync();
            return d.Value;
        }

        /// <summary>
        ///     清空Http缓存表
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ClearCacheAsync()
        {
            var r = _con.ClearTable(nameof(_con.HttpClientCache));
            return await r > 0;
        }

        /// <summary>
        ///     AidCache表中获取或创建
        /// </summary>
        /// <param name="bvid"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<int> GetOrCreateAidCacheAsync(string bvid, Func<Task<int>> factory)
        {
            var a = _con.AidCache.Where(e => e.Bvid.Equals(bvid));
            if (await a.CountAsync() > 0)
            {
                var b = await a.Select(s => s.Aid).FirstOrDefaultAsync();
                return b;
            }

            var c = await factory();
            if (c != 0)
            {
                var d = new AidCacheTable
                {
                    Bvid = bvid,
                    Aid = c
                };
                await _con.AidCache.AddAsync(d);
                await _con.SaveChangesAsync();
            }

            return c;
        }
    }
}
