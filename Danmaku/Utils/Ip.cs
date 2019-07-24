using System.Text;

namespace Danmaku.Utils
{
    public class Ip
    {
        /// <summary>
        ///     Ip转化为10进制
        /// </summary>
        /// <param name="ip">点分隔的ip</param>
        /// <returns>十进制ip</returns>
        public static long Ip2Long(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return 0;
            char[] separator = {'.'};
            var items = ip.Split(separator);
            return (long.Parse(items[0]) << 24)
                   | (long.Parse(items[1]) << 16)
                   | (long.Parse(items[2]) << 8)
                   | long.Parse(items[3]);
        }

        /// <summary>
        ///     十进制ip转换为字符串
        /// </summary>
        /// <param name="ipInt">十进制ip</param>
        /// <returns>字符串ip</returns>
        public static string Long2Ip(long ipInt)
        {
            var sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }
    }
}