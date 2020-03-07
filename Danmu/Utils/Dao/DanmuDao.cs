using System;
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
            var guid = Guid.Parse(id);
            return await _con.Danmu.Where(e => e.Id.Equals(guid)).Include(e => e.Video).FirstOrDefaultAsync();
        }

        /// <summary>
        ///     通过视频Vid查询弹幕
        /// </summary>
        /// <param name="vid">视频vid</param>
        /// <returns>通用弹幕列表</returns>
        public async Task<BaseDanmuData[]> QueryDanmusByVidAsync(string vid)
        {
            return await _con.Danmu.Where(e => e.Vid.Equals(vid) && !e.IsDelete).Select(s => s.Data).ToArrayAsync();
        }

        /// <summary>
        ///     插入弹幕
        /// </summary>
        /// <param name="danmu">弹幕信息</param>
        /// <returns>是否成功</returns>
        public async Task<bool> InsertDanmuAsync(DanmuTable danmu)
        {
            await _con.Danmu.AddAsync(danmu);
            return await _con.SaveChangesAsync() > 0;
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
            dataBase.Data = new BaseDanmuData
            {
                Time = time,
                Mode = mode,
                Color = color,
                Text = text ?? dataBase.Data.Text
            };
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
    }
}
