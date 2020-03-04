using System.Linq;
using System.Threading.Tasks;
using Danmu.Model.Danmu.DanmuData;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Utils.Dao
{
    public class DanmuDao
    {
        private readonly DanmuContext _con;

        public DanmuDao(DanmuContext con)
        {
            _con = con;
        }

        /// <summary>
        ///     通过视频Vid查询弹幕
        /// </summary>
        /// <param name="vid">视频vid</param>
        /// <returns>通用弹幕列表</returns>
        public async Task<BaseDanmuData[]> QueryByVidAsync(string vid)
        {
            return await _con.Danmu.Where(e => e.Vid.Equals(vid) && !e.IsDelete).Select(s => s.Data).ToArrayAsync();
        }

        /// <summary>
        ///     插入弹幕
        /// </summary>
        /// <param name="danmu">弹幕信息</param>
        /// <returns>是否成功</returns>
        public async Task<bool> InsertAsync(DanmuTable danmu)
        {
            await _con.Danmu.AddAsync(danmu);
            return await _con.SaveChangesAsync() > 0;
        }
    }
}
