using System.Collections.Generic;
using Danmaku.Model;

namespace Danmaku.Utils.BiliBili
{
    public interface IBilibiliHelp
    {
        int GetCid(string aid, int p);

        List<DanmakuGet> GetBiDanmaku(string cid);

        List<DanmakuGet> GetBiDanmaku(string cid, string[] date);
    }
}