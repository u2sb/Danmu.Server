using System.IO;
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
        ///     获取视频Cid和分P信息
        /// </summary>
        /// <param name="aid">视频的aid</param>
        /// <returns>Page数据</returns>
        private async Task<BiliBiliPage[]> GetBiliBiliPageAsync(int aid)
        {
            var raw = await GetBiliBiliPageRawAsync($"https://www.bilibili.com/widget/getPageList?aid={aid}");
            if (raw.Length != 0)
            {
                var pages = JsonSerializer.DeserializeAsync<BiliBiliPage[]>(new MemoryStream(raw));
                return await pages;
            }

            return new BiliBiliPage[0];
        }

        /// <summary>
        ///     获取Cid
        /// </summary>
        /// <param name="pages">Page数据</param>
        /// <param name="p">分p</param>
        /// <returns>cid</returns>
        private int GetCid(BiliBiliPage[] pages, int p)
        {
            var cid = 0;
            foreach (var page in pages)
                if (page.Page == p)
                    cid = page.Cid;
            return cid;
        }
    }
}
