using System.IO;
using System.Threading.Tasks;
using Flurl.Http;

namespace Danmu.Utils.BiliBiliHelp
{
    public partial class BiliBiliHelp
    {
        /// <summary>
        ///     获取视频的Page原始数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<Stream> GetBiliBiliPageRawAsync(string path)
        {
            return await _flurlClient.Request(path).GetStreamAsync() ?? Stream.Null;
        }

        /// <summary>
        ///     获取BiliBili弹幕原始数据
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="useCookie"></param>
        /// <returns></returns>
        private async Task<Stream> GetDanmuRawAsync(string path, bool useCookie = false)
        {
            var client = useCookie && !string.IsNullOrEmpty(_setting.Cookie)
                ? _flurlClient.WithHeader("Cookie", _setting.Cookie)
                : _flurlClient;
            return await client.Request(path).GetStreamAsync() ?? Stream.Null;
        }
    }
}
