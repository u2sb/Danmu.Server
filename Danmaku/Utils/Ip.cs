using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Danmaku.Utils
{
    public class Ip
    {
        public static long Ip2Long(string ip)
        {
            if (String.IsNullOrWhiteSpace(ip)) return 0;
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                   | long.Parse(items[1]) << 16
                   | long.Parse(items[2]) << 8
                   | long.Parse(items[3]);
        }
    }
}
