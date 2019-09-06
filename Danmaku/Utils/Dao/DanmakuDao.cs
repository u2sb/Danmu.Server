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
            return con.Danmaku.Where(e => e.Vid == id).Select(s => s.DanmakuData).ToList();
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