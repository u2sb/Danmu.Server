using System.Collections.Generic;
using System.Threading.Tasks;
using Danmaku.Model;

namespace Danmaku.Utils.Dao
{
	public interface IDanmakuDao
	{
		Task<List<DanmakuData>> DanmakuQuery(string id);

		int DanmakuInsert(DanmakuDataInsert date);

		Task<List<DanmakuDataBase>> DanmakuBaseQuery(int page, int size);
		Task<List<DanmakuDataBase>> DanmakuBasesQueryByVid(string vid, int page, int size);
	}
}