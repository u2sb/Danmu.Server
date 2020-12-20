using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using AntDesign.Pro.Layout;
using Danmu.Models.Configs;
using Danmu.Models.Converters;
using Danmu.Utils.BiliBiliHelp;
using Danmu.Utils.Dao;
using Danmu.Utils.Dao.Danmu;
using EasyCaching.LiteDB;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Danmu.Utils.Globals.VariableDictionary;

namespace Danmu
{
    public class Startup
    {
        internal AppSettings AppSetting;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //AppSetting配置文件
            AppSetting = new AppSettings(Configuration);

            //自定义序列化程序
            services.AddControllers().AddXmlSerializerFormatters().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.Converters.Add(new IpAddressConverter());
            });

            //Http请求
            services.AddHttpClient();
            services.AddHttpClient(Gzip).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                {AutomaticDecompression = DecompressionMethods.GZip});
            services.AddHttpClient(Deflate).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                {AutomaticDecompression = DecompressionMethods.Deflate});

            //缓存
            services.AddEasyCaching(options =>
            {
                //use memory cache
                options.UseInMemory(Configuration, InMemory, "EasyCaching:InMemory");
                options.UseLiteDB(config =>
                {
                    config.DBConfig = new LiteDBDBOptions
                    {
                        ConnectionType = ConnectionType.Direct,
                        FilePath = AppSetting.DanmuDb.Directory,
                        FileName = "EasyCaching.db"
                    };
                }, LiteDb);
            });

            services.AddResponseCaching();

            services.AddSignalR();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAntDesign();
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(sp.GetService<NavigationManager>()?.BaseUri ?? string.Empty)
            });
            services.Configure<ProSettings>(Configuration.GetSection("ProSettings"));


            // 转接头，代理
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            //配置跨域
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithMethods("GET", "POST", "OPTIONS"));

                options.AddPolicy(DanmuAllowSpecificOrigins, builder =>
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins(AppSetting.WithOrigins)
                        .WithMethods("GET", "POST", "OPTIONS")
                        .AllowAnyHeader());

                options.AddPolicy(LiveAllowSpecificOrigins, builder =>
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins(AppSetting.LiveWithOrigins)
                        .WithMethods("GET", "POST", "OPTIONS")
                        .AllowAnyHeader().AllowCredentials());
            });

            //注册服务
            services.AddSingleton<AppSettings>();
            services.AddSingleton(l => new LiteDbContext(AppSetting.DanmuDb));

            services.AddScoped<BiliBiliHelp>();
            services.AddScoped<DanmuDao>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                if (AppSetting.UseHttps.UseHsts) app.UseHsts();
            }

            if (AppSetting.UseHttps.UseHttpsRedirection) app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseCors();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
