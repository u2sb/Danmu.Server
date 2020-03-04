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
        public async Task<bool> InsertAsync(string vid, Uri uri)
        {
            var r0 = _con.Video.Where(e => e.Vid.Equals(vid));
            if (await r0.CountAsync() != 0)
            {
                var r1 = r0.Where(e =>
                        e.Referer.Protocol.Equals(uri.Scheme) &&
                        e.Referer.Host.Equals(uri.Host) &&
                        e.Referer.Path.Equals(uri.AbsolutePath) &&
                        e.Referer.Query.Equals(uri.Query) &&
                        e.Referer.Fragment.Equals(uri.Fragment));
                if (await r1.CountAsync() == 0)
                {
                    var video = new VideoTable
                    {
                        Vid = vid,
                        Referer = new Referer(uri)
                    };
                    return await InsertAsync(video);
                }
            }

            return true;
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="video"></param>
        /// <returns>结果</returns>
        public async Task<bool> InsertAsync(VideoTable video)
        {
            await _con.Video.AddAsync(video);
            return await _con.SaveChangesAsync() > 0;
        }
    }
}