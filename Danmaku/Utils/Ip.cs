// Copyright @ 2019 矿冶园
// Product: 实验室安全手机版
// Subsystem: Danmaku
// Author: 迟竞雷
// Time: 2019-07-24 8:54
// Description:

using System.Net;
using System.Numerics;

namespace Danmaku.Utils
{
    public class Ip
    {
        /// <summary>
        ///     Ip转化为10进制
        /// </summary>
        /// <param name="ip">点分隔的ip</param>
        /// <returns>十进制ip</returns>
        public static BigInteger Ip2Long(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return 0;
            if (IPAddress.TryParse(ip, out var a))
                return new BigInteger(a.GetAddressBytes());
            return 0;
        }

        /// <summary>
        ///     十进制ip转换为字符串
        /// </summary>
        /// <param name="ipInt">十进制ip</param>
        /// <returns>字符串ip</returns>
        public static string Long2Ip(BigInteger ipInt)
        {
            var a = ipInt.ToByteArray();
            return new IPAddress(a).ToString();
        }
    }
}