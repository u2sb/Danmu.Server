using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Danmaku.Model.Danmaku;
using Danmaku.Model.DbContext;
using Danmaku.Utils.AppConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Danmaku.Utils.Dao
{
    public class DanmakuDao
    {
        private readonly IAppConfiguration _configuration;

        public DanmakuDao(IAppConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///     根据视频vid查询弹幕
        /// </summary>
        /// <param name="id">视频vid</param>
        /// <returns>弹幕列表</returns>
        public async Task<List<DanmakuData>> DanmakuQuery(string id)
        {
            await using var con = new DanmakuContext(_configuration);
            return await con.Danmaku.Where(e => e.Vid == id && e.IsDelete == false).Select(s => s.DanmakuData)
                            .ToListAsync();
        }

        /// <summary>
        ///     插入弹幕
        /// </summary>
        /// <param name="date">弹幕数据</param>
        /// <returns></returns>
        public async Task<bool> DanmakuInsert(DanmakuDataInsert date)
        {
            await using var con = new DanmakuContext(_configuration);
            var dateBase = new DanmakuDataBase(date);
            await con.Danmaku.AddAsync(dateBase);
            return await con.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     单条弹幕查询
        /// </summary>
        /// <param name="id">弹幕id</param>
        /// <returns>单条弹幕</returns>
        public async Task<DanmakuDataBase> DanmakuBaseQuery(string id)
        {
            await using var con = new DanmakuContext(_configuration);
            if (Guid.TryParse(id, out var guid))
                return await con.Danmaku.Where(e => e.Id == guid).FirstOrDefaultAsync();
            return null;
        }

        /// <summary>
        /// 查询弹幕总数量
        /// </summary>
        /// <returns></returns>
        public int DanmakuBaseQuery()
        {
            using var con = new DanmakuContext(_configuration);
            return con.Danmaku.Count();
        }

        /// <summary>
        ///     全部弹幕查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<DanmakuDataBase[]> DanmakuBaseQuery(int page, int size)
        {
            await using var con = new DanmakuContext(_configuration);
            return await con.Danmaku.OrderBy(b => b.Date)
                            .Skip(size * (page - 1)).Take(size)
                            .ToArrayAsync();
        }

        /// <summary>
        ///     修改弹幕
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<DanmakuDataBase> DanmakuEdit(string id, float time, int type, string color, string text)
        {
            await using var con = new DanmakuContext(_configuration);
            if (Guid.TryParse(id, out var guid))
            {
                var dataBase = await con.Danmaku.Where(e => e.Id == guid).FirstOrDefaultAsync();
                color = color.Replace("#", "");
                dataBase.DanmakuData = new DanmakuData
                {
                    Author = dataBase.DanmakuData.Author,
                    Color = int.TryParse(color, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var c)
                            ? c
                            : dataBase.DanmakuData.Color,
                    Text = text ?? dataBase.DanmakuData.Text,
                    Time = time,
                    Type = type
                };
                con.Danmaku.Update(dataBase);
                if (await con.SaveChangesAsync() > 0) return dataBase;
            }

            return null;
        }

        /// <summary>
        ///     删除弹幕
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<bool> DanmakuDelete(string id)
        {
            await using var con = new DanmakuContext(_configuration);
            if (Guid.TryParse(id, out var guid))
            {
                var dataBase = await con.Danmaku.Where(e => e.Id == guid).FirstOrDefaultAsync();
                dataBase.IsDelete = true;
                con.Danmaku.Update(dataBase);
                return await con.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        ///     筛选弹幕
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">每页弹幕数</param>
        /// <param name="vid">视频id</param>
        /// <param name="author">弹幕发送人</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">弹幕类型</param>
        /// <param name="ip">发送弹幕人ip</param>
        /// <param name="key">弹幕关键词</param>
        /// <param name="order">0-倒序， 1-正序</param>
        /// <returns>筛选结果</returns>
        public async Task<DanmakuDataBase[]> DanmakuBaseQuery(int page, int size, string vid, string author,
                                                                   string startDate,
                                                                   string endDate, int type, string ip,
                                                                   string key, int order)
        {
            IPAddress dip;
            DateTime sDate = DateTime.TryParse(startDate, out sDate) ? sDate : DateTime.MinValue;
            DateTime eDate = DateTime.TryParse(endDate, out eDate) ? eDate : DateTime.MaxValue;
            await using var con = new DanmakuContext(_configuration);
            var a = con.Danmaku.AsNoTracking().Where(d =>
                                (type == 10 || Equals(d.DanmakuData.Type, type)) &&
                                (string.IsNullOrEmpty(ip) || !IPAddress.TryParse(ip, out dip) || Equals(dip, d.Ip)) &&
                                (string.IsNullOrEmpty(startDate) || DateTime.Compare(sDate, d.Date) < 0) &&
                                (string.IsNullOrEmpty(endDate) || DateTime.Compare(eDate, d.Date) > 0) &&
                                (string.IsNullOrEmpty(vid) || d.Vid.Contains(vid)) &&
                                (string.IsNullOrEmpty(author) || d.DanmakuData.Author.Contains(author)) &&
                                (string.IsNullOrEmpty(key) || d.DanmakuData.Text.Contains(key)))
                       .OrderBy(b => b.Vid);
            a = order == 0 ? a.ThenByDescending(b => b.Date) : a.ThenBy(b => b.Date);
            // return await a.Skip(size * (page - 1)).Take(size).ToListAsync();
            return await a.ToArrayAsync();
        }
    }
}
