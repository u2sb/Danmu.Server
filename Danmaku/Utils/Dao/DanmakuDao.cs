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
			return con.Danmaku.Where(e => e.Vid == id).Where(e => e.IsDelete == false).Select(s => s.DanmakuData).ToList();
		}

		public int DanmakuInsert(DanmakuDataInsert date)
		{
			using var con = new DanmakuContext();
			var dateBase = new DanmakuDataBase(date);
			con.Danmaku.Add(dateBase);
			return con.SaveChanges();
		}

		public List<DanmakuDataBase> DanmakuBaseQuery(int page, int size)
		{
			using var con = new DanmakuContext();
			return con.Danmaku
				.OrderBy(b => b.Vid).ThenByDescending(b => b.Date)
				.Skip(size * (page - 1)).Take(size)
				.ToList();
		}

		public List<DanmakuDataBase> DanmakuBasesQueryByVid(string vid, int page, int size)
		{
			using var con = new DanmakuContext();
			return con.Danmaku.Where(e => e.Vid == vid)
				.OrderByDescending(b => b.Date)
				.Skip(size * (page - 1)).Take(size)
				.ToList();
		}
	}
}