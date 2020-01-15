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
		/// <returns></returns>
		Task<bool> DanmakuInsert(DanmakuDataInsert date);

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

		/// <summary>
		/// 修改弹幕
		/// </summary>
		/// <param name="id"></param>
		/// <param name="time"></param>
		/// <param name="type"></param>
		/// <param name="color"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		Task<DanmakuDataBase> DanmakuEdit(string id, float time, int type, string color, string text);

		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="id">id</param>
		/// <returns></returns>
		Task<bool> DanmakuDelete(string id);

		/// <summary>
		/// 筛选弹幕
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
		Task<List<DanmakuDataBase>> DanmakuBasesQuery(int page, int size, string vid, string author, string startDate,
			string endDate, int type, string ip,
			string key, int order);
	}
}