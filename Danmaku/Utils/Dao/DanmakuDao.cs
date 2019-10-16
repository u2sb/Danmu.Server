using System.Collections.Generic;
using System.Linq;
using Danmaku.Model;
using Danmaku.Utils.AppConfiguration;

namespace Danmaku.Utils.Dao
{
	public class DanmakuDao : IDanmakuDao
	{
		private readonly IAppConfiguration _configuration;

		public DanmakuDao(IAppConfiguration configuration)
		{
			_configuration = configuration;
		}

		public List<DanmakuData> DanmakuQuery(string id)
		{
			using var con = new DanmakuContext(_configuration);
			return con.Danmaku.Where(e => e.Vid == id && e.IsDelete == false).Select(s => s.DanmakuData).ToList();
		}

		public int DanmakuInsert(DanmakuDataInsert date)
		{
			using var con = new DanmakuContext(_configuration);
			var dateBase = new DanmakuDataBase(date);
			con.Danmaku.AddAsync(dateBase);
			return con.SaveChanges();
		}

		public List<DanmakuDataBase> DanmakuBaseQuery(int page, int size)
		{
			using var con = new DanmakuContext(_configuration);
			return con.Danmaku
				.OrderBy(b => b.Vid).ThenByDescending(b => b.Date)
				.Skip(size * (page - 1)).Take(size)
				.ToList();
		}

		public List<DanmakuDataBase> DanmakuBasesQueryByVid(string vid, int page, int size)
		{
			using var con = new DanmakuContext(_configuration);
			return con.Danmaku.Where(e => e.Vid == vid)
				.OrderByDescending(b => b.Date)
				.Skip(size * (page - 1)).Take(size)
				.ToList();
		}
	}
}