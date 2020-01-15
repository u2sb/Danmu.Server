using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
			return await con.Danmaku.Where(e => e.Vid == id && e.IsDelete == false).Select(s => s.DanmakuData)
				.ToListAsync();
		}

		public async Task<bool> DanmakuInsert(DanmakuDataInsert date)
		{
			await using var con = new DanmakuContext(_configuration);
			var dateBase = new DanmakuDataBase(date);
			await con.Danmaku.AddAsync(dateBase);
			return await con.SaveChangesAsync() > 0;
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

		public async Task<List<DanmakuDataBase>> DanmakuBasesQuery(int page, int size, string vid, string author,
			string startDate,
			string endDate, int type, string ip,
			string key, int order)
		{
			IPAddress dip;
			DateTime sDate = DateTime.TryParse(startDate, out sDate) ? sDate : DateTime.MinValue;
			DateTime eDate = DateTime.TryParse(startDate, out eDate) ? eDate : DateTime.MaxValue;
			await using var con = new DanmakuContext(_configuration);
			var a = con.Danmaku.AsNoTracking().Where(d =>
					(string.IsNullOrEmpty(vid) || d.Vid.Contains(vid)) &&
					(string.IsNullOrEmpty(author) || d.DanmakuData.Author.Contains(author)) &&
					(type == 10 || d.DanmakuData.Type == type) &&
					(string.IsNullOrEmpty(ip) || !IPAddress.TryParse(ip, out dip) || Equals(dip, d.Ip)) &&
					d.Date >= sDate && d.Date <= eDate &&
					(string.IsNullOrEmpty(key) || d.DanmakuData.Text.Contains(key)))
				.OrderBy(b => b.Vid);
			a = order == 0 ? a.ThenByDescending(b => b.Date) : a.ThenBy(b => b.Date);
			return await a.Skip(size * (page - 1)).Take(size).ToListAsync();
		}
	}
}