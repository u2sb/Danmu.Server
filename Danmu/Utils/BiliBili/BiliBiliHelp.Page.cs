using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Danmu.Model.Danmu.BiliBili;

namespace Danmu.Utils.BiliBili
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
        private async Task<BiliBiliPage> GetBiliBiliPageAsync(int aid)
        {
            var raw = await GetBiliBiliPageRawAsync($"https://api.bilibili.com/x/player/pagelist?aid={aid}");
            if (raw.Length != 0)
            {
                var pages = JsonSerializer.DeserializeAsync<BiliBiliPage>(new MemoryStream(raw));
                return await pages;
            }

            return new BiliBiliPage();
        }

        /// <summary>
        ///     获取视频Cid和分P信息
        /// </summary>
        /// <param name="bvid"></param>
        /// <returns></returns>
        private async Task<BiliBiliPage> GetBiliBiliPageAsync(string bvid)
        {
            var raw = await GetBiliBiliPageRawAsync($"https://api.bilibili.com/x/player/pagelist?bvid={bvid}");
            if (raw.Length != 0)
            {
                var pages = JsonSerializer.DeserializeAsync<BiliBiliPage>(new MemoryStream(raw));
                return await pages;
            }

            return new BiliBiliPage();
        }

        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="pages">Page数据</param>
        /// <param name="p">分p</param>
        /// <returns>cid</returns>
        private int GetCid(BiliBiliPage pages, int p)
        {
            var cid = pages.Code == 0 ? pages.Data.Where(e => e.Page == p).Select(s => s.Cid).FirstOrDefault() : 0;
            return cid;
        }
    }
}
