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
  ///   UnixSocket
  /// </summary>
  public string UnixSocket { get; init; } = string.Empty;

  /// <summary>
  ///   Port
  /// </summary>
  public ushort Port { get; init; } = 3567;

  public string PidFile { get; init; } = "./DanMu.pid";

  /// <summary>
  ///   跨域设置
  /// </summary>
  public string[] WithOrigins { get; init; } = [];

  /// <summary>
  ///   数据库设置
  /// </summary>
  public DataBase DataBase { get; init; } = new();

  /// <summary>
  ///   BiliBili弹幕解析相关设置
  /// </summary>
  public BiliBiliSetting BiliBiliSetting { get; init; } = new();
}

/// <summary>
///   数据库相关设置
/// </summary>
public class DataBase
{
  public string Directory { get; init; } = "DataBase";
  public string CachingDb { get; init; } = "Caching.cache";
  
  public int PoolSize { get; init; } = 8;
}

/// <summary>
///   BiliBili弹幕解析相关设置
/// </summary>
public class BiliBiliSetting
{
  /// <summary>
  ///   Cid缓存时间 单位h
  /// </summary>
  public int PageCacheTime { get; init; } = 8640;

  /// <summary>
  ///   弹幕缓存时间 单位h
  /// </summary>
  public int DanMuCacheTime { get; init; } = 6;

  /// <summary>
  ///   Cookie
  /// </summary>
  public string? Cookie { get; init; }
}