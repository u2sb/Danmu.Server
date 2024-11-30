using DanMu.Models.Settings;
using DanMu.Utils.Caching;

namespace DanMu.Utils.Program;

public class SbLife(AppSettings appSettings, CachingContext cachingContext)
{
  public void Register(IHostApplicationLifetime life)
  {
    life.ApplicationStarted.Register(OnStarted);
    life.ApplicationStopping.Register(OnStopping);
    life.ApplicationStopped.Register(OnStopped);
  }


  private void OnStarted()
  {
    // PID文件
    if (!string.IsNullOrWhiteSpace(appSettings.PidFile))
    {
      var pid = Environment.ProcessId;
      File.WriteAllText(appSettings.PidFile, pid.ToString());
    }

    // 数据库确保表被创建
    cachingContext.Database.EnsureCreated();
  }

  private void OnStopping()
  {
  }

  private void OnStopped()
  {
    // 删除文件
    if (File.Exists(appSettings.PidFile)) File.Delete(appSettings.PidFile);
    if (!string.IsNullOrWhiteSpace(appSettings.UnixSocket) & File.Exists(appSettings.UnixSocket))
      File.Delete(appSettings.UnixSocket);
  }
}