using System.Collections.Generic;
using System.Threading.Tasks;
using Danmaku.Model;

namespace Danmaku.Utils.Dao
{
	public interface IDanmakuDao
	{
		/// <summary>
		/// 根据视频vid查询弹幕
		/// </summary>
		/// <param name="id">视频vid</param>
		/// <returns>弹幕列表</returns>
		Task<List<DanmakuData>> DanmakuQuery(string id);

		/// <summary>
		/// 插入弹幕
		/// </summary>
		/// <param name="date">弹幕数据</param>
		/// <returns>0失败, 1成功</returns>
		int DanmakuInsert(DanmakuDataInsert date);

		/// <summary>
		/// 单条弹幕查询
		/// </summary>
		/// <param name="id">弹幕id</param>
		/// <returns>单条弹幕</returns>
		Task<DanmakuDataBase> DanmakuBaseQuery(string id);

		/// <summary>
		/// 全部弹幕查询
		/// </summary>
		/// <param name="page"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		Task<List<DanmakuDataBase>> DanmakuBaseQuery(int page, int size);

		/// <summary>
		/// 根据vid筛选全部视频
		/// </summary>
		/// <param name="vid"></param>
		/// <param name="page"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		Task<List<DanmakuDataBase>> DanmakuBasesQueryByVid(string vid, int page, int size);
	}
}