using System.IO;
using System.Net;
using CommandLine;
using Danmu.Models.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Danmu
{
    public class Program
    {
        internal static AppSettings AppSettings;

        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args).WithParsed(opts =>
            {
                var host = CreateHostBuilder(args).Build();

                if (!string.IsNullOrEmpty(opts.UserName) && !string.IsNullOrEmpty(opts.Password))
                {
                    //这里初始化用户名和密码
                }
                else
                {
                    //正常启动
                    DbInit(AppSettings.DanmuDb);
                    host.Run();
                }
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    builder
                        .AddJsonFile("appsettings.json", true, true)
                        .AddYamlFile("appsettings.yml", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.yml", true);
                    AppSettings = builder.Build().Get<AppSettings>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        var ks = AppSettings.KestrelSettings;
#if LINUX
                                if (ks.UnixSocketPath.Length > 0)
                                    foreach (var path in ks.UnixSocketPath)
                                    {
                                        if (File.Exists(path)) File.Delete(path);
                                        options.ListenUnixSocket(path);
                                    }
#endif
                        if (ks.Listens.Count > 0)
                            foreach (var listen in ks.Listens)
                                if (IPAddress.TryParse(listen.Key, out var ip))
                                    foreach (var port in listen.Value)
                                        options.Listen(ip, port);
                    }).UseStartup<Startup>();
                });
        }

        private static void DbInit(DanmuDb db)
        {
            if (!Directory.Exists(db.Directory)) Directory.CreateDirectory(db.Directory);
        }

        private static void RunOptions(CommandOptions opts)
        {
        }
    }
}
