using System;
using System.Net;
using Danmu.Model.Config;
using Danmu.Model.DbContext;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
#if LINUX
using System.IO;
#endif

namespace Danmu
{
    public class Program
    {
        private static AppSettings _appSettings = new AppSettings();

        private static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
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
                            _appSettings = builder.Build().Get<AppSettings>();
                        })
                       .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.ConfigureKestrel(options =>
                            {
                                var ks = _appSettings.KestrelSettings;
#if LINUX
                                if (ks.UnixSocketPath.Length > 0)
                                    foreach (var path in ks.UnixSocketPath)
                                    {
                                        if (File.Exists(path)) File.Delete(path);
                                        options.ListenUnixSocket(path);
                                    }
#endif
                                if (ks.Port.Length > 0)
                                    foreach (var port in ks.Port)
                                        options.Listen(IPAddress.Loopback, port);
                            }).UseStartup<Startup>();
                        });
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DanmuContext>();
                DbInitializer.Initialize(context, _appSettings);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }
}
