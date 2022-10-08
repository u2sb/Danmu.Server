using Meting4Net.Core;

namespace DanMu.Models.Settings;

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
    ///     跨域设置
    /// </summary>
    public string[] WithOrigins { get; set; } = { };

    /// <summary>
    ///     BiliBili弹幕解析相关设置
    /// </summary>
    public BiliBiliSetting BiliBiliSetting { get; set; } = new();

    /// <summary>
    ///     数据库设置
    /// </summary>
    public DataBase DataBase { get; set; } = new();

    /// <summary>
    ///     管理用户，临时解决方案
    /// </summary>
    public Admin[] Admins { get; set; } = Array.Empty<Admin>();


    /// <summary>
    ///     Meting
    /// </summary>
    public Meting Meting { get; set; } = new();
}

/// <summary>
///     BiliBili弹幕解析相关设置
/// </summary>
public class BiliBiliSetting
{
    /// <summary>
    ///     Cid缓存时间 单位h
    /// </summary>
    public int PageCacheTime { get; set; } = 8640;

    /// <summary>
    ///     弹幕缓存时间 单位h
    /// </summary>
    public int DanMuCacheTime { get; set; } = 6;
}

/// <summary>
///     数据库相关设置
/// </summary>
public class DataBase
{
    public string Directory { get; set; } = "DataBase";
    public string CachingDb { get; set; } = "Caching.db";
    public string DanMuDb { get; set; } = "DanMu.db";
}

public class Admin
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class Meting
{
    public ServerProvider DefaultServerProvider { get; set; } = ServerProvider.Tencent;
    public string Url { get; set; }
    public MetingCachingTime CachingTime { get; set; } = new();
    public MetingReplace Replace { get; set; } = new();
}

public class MetingCachingTime
{
    public int Base { get; set; } = 120;
    public int Url { get; set; } = 120;
    public int Pic { get; set; } = 120;
    public int Lrc { get; set; } = 43200;
}

public class MetingReplace
{
    public List<List<string>>? Url { get; set; }
    public List<List<string>>? Pic { get; set; }
    public List<List<string>>? Lrc { get; set; }
}