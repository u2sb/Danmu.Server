using System;
using System.Collections.Generic;
using System.Linq;
using Danmaku.Model;

namespace Danmaku.Utils.Dao
{
    public class DanmakuDao : IDanmakuDao
    {
        public List<DanmakuData> DanmakuQuery(string id)
        {
            using var con = new DanmakuContext();
            return con.Danmaku.Where(e => e.Danmaku.Id == id).Select(s => new DanmakuData
            {
                Time = s.Danmaku.DanmakuData.Time,
                Type = s.Danmaku.DanmakuData.Type,
                Color = s.Danmaku.DanmakuData.Color,
                Author = s.Danmaku.DanmakuData.Author,
                Text = s.Danmaku.DanmakuData.Text
            }).ToList();
        }

        public int DanmakuInsert(DanmakuDataInsert date)
        {
            using var con = new DanmakuContext();
            var dateBase = new DanmakuDataBase(date);
            con.Danmaku.Add(dateBase);
            return con.SaveChanges();
        }
    }
}