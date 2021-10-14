using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Danmu.Models.Danmus.BiliBili;

namespace Danmu.Utils.BiliBiliHelp
{
    public partial class BiliBiliHelp
    {
        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="aid">视频的aid</param>
        /// <param name="p">分p</param>
        /// <returns>cid</returns>
        public async Task<int> GetCidAsync(int aid, int p)
        {
            var pages = await GetBiliBiliPageAsync(aid);
            return GetCid(pages, p);
        }

        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="bvid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<int> GetCidAsync(string bvid, int p)
        {
            var pages = await GetBiliBiliPageAsync(bvid);
            return GetCid(pages, p);
        }

        /// <summary>
        ///     获取视频Cid和分P信息
        /// </summary>
        /// <param name="aid">视频的aid</param>
        /// <returns>Page数据</returns>
        public async Task<BiliBiliPage> GetBiliBiliPageAsync(int aid)
        {
            return await _caching.GetAsync($"{nameof(GetBiliBiliPageAsync)}{aid}",
                async () =>
                {
                    var a = await GetBiliBiliPageRawAsync($"/x/player/pagelist?aid={aid}");
                    if (a != Stream.Null) return await JsonSerializer.DeserializeAsync<BiliBiliPage>(a);
                    return null;
                }, TimeSpan.FromHours(_setting.CidCacheTime));
        }

        /// <summary>
        ///     获取视频Cid和分P信息
        /// </summary>
        /// <param name="bvid"></param>
        /// <returns></returns>
        public async Task<BiliBiliPage> GetBiliBiliPageAsync(string bvid)
        {
            return await _caching.GetAsync($"{nameof(GetBiliBiliPageAsync)}{bvid}",
                async () =>
                {
                    var a = await GetBiliBiliPageRawAsync(
                        $"/x/player/pagelist?bvid={bvid}");
                    if (a != Stream.Null) return await JsonSerializer.DeserializeAsync<BiliBiliPage>(a);
                    return null;
                }, TimeSpan.FromHours(_setting.CidCacheTime));
        }

        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="pages">Page数据</param>
        /// <param name="p">分p</param>
        /// <returns>cid</returns>
        private int GetCid(BiliBiliPage pages, int p)
        {
            var cid = pages?.Code == 0 ? pages.Data.Where(e => e.Page == p).Select(s => s.Cid).FirstOrDefault() : 0;
            return cid;
        }
    }
}
