using System;
using System.Linq;
using System.Threading.Tasks;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Utils.Dao
{
    public class VideoDao
    {
        private readonly DanmuContext _con;

        public VideoDao(DanmuContext con)
        {
            _con = con;
        }

        /// <summary>
        ///     判断是否已存在并插入数据
        /// </summary>
        /// <param name="vid">视频的vid</param>
        /// <param name="uri">uri</param>
        /// <returns>结果</returns>
        public async Task<VideoTable> InsertAsync(string vid, Uri uri)
        {
            var r0 = _con.Video.Where(e => e.Vid.Equals(vid));

            var r1 = r0.Where(e =>
                    e.Referer.Protocol.Equals(uri.Scheme) &&
                    e.Referer.Host.Equals(uri.Host) &&
                    e.Referer.Path.Equals(uri.AbsolutePath) &&
                    (string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(e.Referer.Query) ||
                     e.Referer.Query.Equals(uri.Query)) &&
                    (string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(e.Referer.Query) ||
                     e.Referer.Query.Equals(uri.Query)));

            if (await r0.CountAsync() == 0 || await r1.CountAsync() == 0)
            {
                var video = new VideoTable
                {
                    Vid = vid,
                    Referer = new Referer(uri)
                };
                return await InsertAsync(video);
            }

            return r0.FirstOrDefault();
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="video"></param>
        /// <returns>结果</returns>
        public async Task<VideoTable> InsertAsync(VideoTable video)
        {
            var a = await _con.Video.AddAsync(video);
            if (await _con.SaveChangesAsync() > 0)
                return a.Entity;
            return null;
        }

        /// <summary>
        ///     获取vid列表
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> GetVidsAsync()
        {
            return await _con.Video.AsNoTracking().Select(e => e.Vid).Distinct().ToArrayAsync();
        }
    }
}
