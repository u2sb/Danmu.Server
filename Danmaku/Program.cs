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
#if DEBUG || WIN
                        options.ListenAnyIP(5000);
#elif LINUX
                        options.ListenUnixSocket("/tmp/dplayer.danmaku.sock");
#endif
                    });
                });
    }
}