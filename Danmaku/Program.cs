#if LINUX
using System.IO;
#endif
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Danmaku
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().ConfigureKestrel((context, options) =>
                    {
#if LINUX
                        if (File.Exists("/tmp/dplayer.danmaku.sock")) File.Delete("/tmp/dplayer.danmaku.sock");
                        options.ListenUnixSocket("/tmp/dplayer.danmaku.sock");
#endif
                    });
                });
    }
}