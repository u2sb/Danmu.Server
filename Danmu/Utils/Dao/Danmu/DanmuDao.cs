using System.Threading.Tasks;
using Danmu.Models.Danmus.Danmu;
using Danmu.Models.Danmus.DataTables;
using LiteDB;

namespace Danmu.Utils.Dao.Danmu
{
    public class DanmuDao
    {
        private readonly ILiteCollection<DanmuTable> _danmuTable;

        public DanmuDao(LiteDbContext con)
        {
            _danmuTable = con.Database.GetCollection<DanmuTable>();
        }

        /// <summary>
        ///     通过视频Vid查询弹幕
        /// </summary>
        /// <param name="vid">视频vid</param>
        /// <returns>通用弹幕列表</returns>
        public async Task<BaseDanmuData[]> QueryDanmusByVidAsync(string vid)
        {
            var data = Task.Run(() =>
            {
                return _danmuTable.Query().Where(e => e.Vid.Equals(vid) && !e.IsDelete).Select(s => s.Data);
            });

            return (await data).ToArray();
        }

        /// <summary>
        ///     插入弹幕
        /// </summary>
        /// <param name="danmu">弹幕信息</param>
        /// <returns>是否成功</returns>
        public async Task<bool> InsertDanmuAsync(DanmuTable danmu)
        {
            return await Task.Run(() => _danmuTable.Insert(danmu));
        }
    }
}
