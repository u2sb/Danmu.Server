using System.Diagnostics;
using DanMu.Models.Settings;
using DanMu.Utils.Caching;

namespace DanMu.Utils.Program;

public class SbLife
{
  private readonly CachingContext _achingContext;
  private readonly AppSettings _appSettings;

  public SbLife(AppSettings appSettings, CachingContext cachingContext)
  {
    _appSettings = appSettings;
    _achingContext = cachingContext;
  }

  public void Register(IHostApplicationLifetime life)
  {
    life.ApplicationStarted.Register(OnStarted);
    life.ApplicationStopping.Register(OnStopping);
    life.ApplicationStopped.Register(OnStopped);
  }


  public void OnStarted()
  {
    // PID文件
    if (!string.IsNullOrWhiteSpace(_appSettings.PidFile))
    {
      var pid = Process.GetCurrentProcess().Id;
      File.WriteAllText(_appSettings.PidFile, pid.ToString());
    }
  }

  public void OnStopping()
  {
    _achingContext.Database.Dispose();
  }

  public void OnStopped()
  {
    // 删除文件
    if (File.Exists(_appSettings.PidFile)) File.Delete(_appSettings.PidFile);
    if (!string.IsNullOrWhiteSpace(_appSettings.UnixSocket) & File.Exists(_appSettings.UnixSocket))
      File.Delete(_appSettings.UnixSocket);
  }
}