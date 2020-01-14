using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Danmaku.Model;
using Danmaku.Utils.AppConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Danmaku.Utils.Dao
{
	public class DanmakuDao : IDanmakuDao
	{
		private readonly IAppConfiguration _configuration;

		public DanmakuDao(IAppConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<List<DanmakuData>> DanmakuQuery(string id)
		{
			await using var con = new DanmakuContext(_configuration);
			return await con.Danmaku.Where(e => e.Vid == id && e.IsDelete == false).Select(s => s.DanmakuData).ToListAsync();
		}

		public int DanmakuInsert(DanmakuDataInsert date)
		{
			using var con = new DanmakuContext(_configuration);
			var dateBase = new DanmakuDataBase(date);
			con.Danmaku.AddAsync(dateBase);
			return con.SaveChanges();
		}

		public async Task<DanmakuDataBase> DanmakuBaseQuery(string id)
		{
			await using var con = new DanmakuContext(_configuration);
			if (Guid.TryParse(id, out var guid))
				return await con.Danmaku.Where(e => e.Id == guid).FirstOrDefaultAsync();
			return null;
		}

		public async Task<List<DanmakuDataBase>> DanmakuBaseQuery(int page, int size)
		{
			await using var con = new DanmakuContext(_configuration);
			return await con.Danmaku
				.OrderBy(b => b.Vid).ThenByDescending(b => b.Date)
				.Skip(size * (page - 1)).Take(size)
				.ToListAsync();
		}

		public async Task<List<DanmakuDataBase>> DanmakuBasesQueryByVid(string vid, int page, int size)
		{
			await using var con = new DanmakuContext(_configuration);
			return await con.Danmaku.Where(e => e.Vid == vid)
				.OrderByDescending(b => b.Date)
				.Skip(size * (page - 1)).Take(size)
				.ToListAsync();
		}
	}
}