using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Danmu.Models.Configs
{
    public class AppSettings
    {
        public AppSettings()
        {
        }

        public AppSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        /// <summary>
        ///     Kestrel设置
        /// </summary>
        public KestrelSettings KestrelSettings { get; set; } = new();

        /// <summary>
        ///     跨域设置
        /// </summary>
        public string[] WithOrigins { get; set; }

        /// <summary>
        ///     直播跨域设置
        /// </summary>
        public string[] LiveWithOrigins { get; set; }

        /// <summary>
        ///     Https相关设置
        /// </summary>
        public UseHttps UseHttps { get; set; } = new();

        /// <summary>
        ///     弹幕数据库设置
        /// </summary>
        public DanmuDb DanmuDb { get; set; } = new();

        /// <summary>
        ///     BiliBili弹幕解析相关设置
        /// </summary>
        public BiliBiliSetting BiliBiliSetting { get; set; } = new();
    }

    public class DanmuDb
    {
        /// <summary>
        ///     数据库目录
        /// </summary>
        public string Directory { get; set; } = "./DataBase/";
    }

    public class BiliBiliSetting
    {
        /// <summary>
        ///     Cookie
        /// </summary>
        public string Cookie { get; set; }

        /// <summary>
        ///     Cid缓存时间 单位h
        /// </summary>
        public int CidCacheTime { get; set; } = 72;

        /// <summary>
        ///     弹幕缓存时间 单位h
        /// </summary>
        public int DanmuCacheTime { get; set; } = 5;
    }


    public class KestrelSettings
    {
        /// <summary>
        ///     监听地址
        /// </summary>
        public Dictionary<string, int[]> Listens { get; set; }

        /// <summary>
        ///     UnixSocketPath
        /// </summary>
        public string[] UnixSocketPath { get; set; }
    }

    public class UseHttps
    {
        public bool UseHttpsRedirection { get; set; } = false;

        public bool UseHsts { get; set; } = false;
    }
}
