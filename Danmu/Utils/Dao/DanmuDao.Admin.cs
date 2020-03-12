using System;
using System.Linq;
using System.Threading.Tasks;
using Danmu.Model.DataTable;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Utils.Dao
{
    public partial class DanmuDao
    {
        /// <summary>
        ///     获取全部弹幕的数量
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAllDanmuAsync()
        {
            return await _con.Danmu.CountAsync();
        }

        /// <summary>
        ///     分页查询全部弹幕
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public async Task<DanmuTable[]> GetAllDanmuAsync(int page, int size, bool descending = true)
        {
            var allDanmu = _con.Danmu;
            var order = descending
                    ? allDanmu.OrderByDescending(b => b.CreateTime)
                    : allDanmu.OrderBy(b => b.CreateTime);
            return await order.Skip(size * (page - 1)).Take(size).ToArrayAsync();
        }

        /// <summary>
        ///     通过id获取相应的弹幕
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DanmuTable> QueryDanmuByIdAsync(string id)
        {
            if (Guid.TryParse(id, out var guid))
                return await _con.Danmu.Where(e => e.Id.Equals(guid)).Include(e => e.Video).FirstOrDefaultAsync();
            return new DanmuTable();
        }

        /// <summary>
        ///     编辑弹幕
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="mode"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<DanmuTable> EditDanmuAsync(Guid id, float time, int mode, int color, string text)
        {
            var dataBase = await _con.Danmu.Where(e => e.Id.Equals(id)).FirstOrDefaultAsync();
            dataBase.Data.Time = time;
            dataBase.Data.Mode = mode;
            dataBase.Data.Color = color;
            dataBase.Data.Text = text ?? dataBase.Data.Text;
            dataBase.UpdateTime = DateTime.UtcNow;
            _con.Update(dataBase);
            if (await _con.SaveChangesAsync() > 0) return dataBase;
            return null;
        }

        /// <summary>
        ///     删除弹幕
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDanmuAsync(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                var dataBase = await _con.Danmu.Where(e => e.Id == guid).FirstOrDefaultAsync();
                dataBase.UpdateTime = DateTime.UtcNow;
                dataBase.IsDelete = true;
                _con.Danmu.Update(dataBase);
                return await _con.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        ///     通过vid查询到的弹幕总数
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        public async Task<int> GetDanmuByVidAsync(string vid)
        {
            return await _con.Danmu.Where(e => e.Vid.Equals(vid)).CountAsync();
        }

        /// <summary>
        ///     通过vid查询弹幕表
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="size"></param>
        /// <param name="descending"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<DanmuTable[]> GetDanmuByVidAsync(string vid, int page, int size,
                                                           bool descending = true)
        {
            var allDanmu = _con.Danmu.Where(e => e.Vid.Equals(vid));
            var order = descending
                    ? allDanmu.OrderByDescending(b => b.CreateTime)
                    : allDanmu.OrderBy(b => b.CreateTime);
            return await order.Skip(size * (page - 1)).Take(size).ToArrayAsync();
        }
    }
}
