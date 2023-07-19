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
  public string UnixSocket { get; set; } = string.Empty;

  /// <summary>
  ///   Port
  /// </summary>
  public int Port { get; set; } = 3567;

  public string PidFile { get; set; } = "./DanMu.pid";

  /// <summary>
  ///   跨域设置
  /// </summary>
  public string[] WithOrigins { get; set; } = Array.Empty<string>();

  /// <summary>
  ///   数据库设置
  /// </summary>
  public DataBase DataBase { get; set; } = new();

  /// <summary>
  ///   BiliBili弹幕解析相关设置
  /// </summary>
  public BiliBiliSetting BiliBiliSetting { get; set; } = new();
}

/// <summary>
///   数据库相关设置
/// </summary>
public class DataBase
{
  public string Directory { get; set; } = "DataBase";
  public string CachingDb { get; set; } = "Caching.cache";
}

/// <summary>
///   BiliBili弹幕解析相关设置
/// </summary>
public class BiliBiliSetting
{
  /// <summary>
  ///   Cid缓存时间 单位h
  /// </summary>
  public int PageCacheTime { get; set; } = 8640;

  /// <summary>
  ///   弹幕缓存时间 单位h
  /// </summary>
  public int DanMuCacheTime { get; set; } = 6;
}