using System.Collections.Generic;
using Danmaku.Model;

namespace Danmaku.Utils.MySql
{
    public interface IDanmakuDao
    {
        List<DanmakuGet> Query(string id);

        int Insert(DanmakuModel danmaku);
    }
}