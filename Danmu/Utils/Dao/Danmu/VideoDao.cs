using System;
using System.Threading.Tasks;
using Danmu.Models.Danmus.DataTables;
using LiteDB;

namespace Danmu.Utils.Dao.Danmu
{
    public class VideoDao
    {
        private readonly ILiteCollection<VideoTable> _videoTable;

        public VideoDao(LiteDbContext con)
        {
            _videoTable = con.Database.GetCollection<VideoTable>();
        }

        /// <summary>
        ///     判断是否已存在并插入数据
        /// </summary>
        /// <param name="vid">视频的vid</param>
        /// <param name="uri">uri</param>
        /// <returns>结果</returns>
        public VideoTable Insert(string vid, Uri uri)
        {
            var r0 = _videoTable.Query().Where(e => e.Vid.Equals(vid));

            var r1 = r0.Where(e =>
                e.Referer.Protocol.Equals(uri.Scheme) &&
                e.Referer.Host.Equals(uri.Host) &&
                e.Referer.Path.Equals(uri.AbsolutePath) &&
                (string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(e.Referer.Query) ||
                 e.Referer.Query.Equals(uri.Query)) &&
                (string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(e.Referer.Query) ||
                 e.Referer.Query.Equals(uri.Query)));

            if (r0.Count() == 0 || r1.Count() == 0)
            {
                var video = new VideoTable
                {
                    Vid = vid,
                    Referer = new Referer(uri)
                };
                return Insert(video);
            }

            return r0.FirstOrDefault();
        }

        /// <summary>
        ///     插入数据
        /// </summary>
        /// <param name="video"></param>
        /// <returns>结果</returns>
        public VideoTable Insert(VideoTable video)
        {
            var a = _videoTable.Insert(video);
            if (a)
                return video;
            return null;
        }

        /// <summary>
        ///     获取vid列表
        /// </summary>
        /// <returns></returns>
        public string[] GetVidsAsync()
        {
            return _videoTable.Query().Select(e => e.Vid).ToArray();
        }
    }
}
