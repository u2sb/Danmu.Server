using System.Collections.Generic;
using Danmaku.Model;

namespace Danmaku.Utils.PostgreSQL
{
    public interface IDanmakuDao
    {
        /// <summary>
        ///     查询函数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<string> Query(string id);

        /// <summary>
        ///     插入函数
        /// </summary>
        /// <param name="danmaku"></param>
        /// <returns></returns>
        int Insert(DanmakuInsert danmaku);
    }
}