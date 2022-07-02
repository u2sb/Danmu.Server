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
    public string DanMuCachingDb { get; set; } = "DanMuCaching.db";
    public string DanMuDb { get; set; } = "DanMu.db";
    public int PoolSize { get; set; } = 5;
}