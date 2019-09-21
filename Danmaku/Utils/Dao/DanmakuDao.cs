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
			return con.Danmaku.Where(e => e.Vid == id).Select(s => s.DanmakuData).ToList();
		}

		public int DanmakuInsert(DanmakuDataInsert date)
		{
			using var con = new DanmakuContext();
			var dateBase = new DanmakuDataBase(date);
			con.Danmaku.Add(dateBase);
			return con.SaveChanges();
		}

		public List<DanmakuDataBase> DanmakuBaseQuery()
		{
			using var con = new DanmakuContext();
			return con.Danmaku.OrderBy(b => b.Date).ToList();
		}

		public List<DanmakuDataBase> DanmakuBasesQueryByVid(string vid)
		{
			using var con = new DanmakuContext();
			return con.Danmaku.Where(e => e.Vid == vid).OrderBy(b => b.Date).ToList();
		}
	}
}