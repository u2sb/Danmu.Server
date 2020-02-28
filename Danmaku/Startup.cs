using System;
using System.Net;
using System.Net.Http;
using Danmaku.Model.DbContext;
using Danmaku.Utils;
using Danmaku.Utils.AppConfiguration;
using Danmaku.Utils.BiliBili;
using Danmaku.Utils.Dao;
using Danmaku.Utils.LiveDanmaku;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if DEBUG
using VueCliMiddleware;

#endif

namespace Danmaku
{
    public class Startup
    {
        private readonly string DanmakuAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly string LiveAllowSpecificOrigins = "_myLiveAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //全局配置文件
            var config = new AppConfiguration(Configuration);
            services.AddSingleton<IAppConfiguration>(s => config);

            //数据库
            services.AddDbContext<DanmakuContext>();

            //http请求
            services.AddHttpClient("gzip").ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {AutomaticDecompression = DecompressionMethods.GZip});
            services.AddHttpClient("deflate").ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {AutomaticDecompression = DecompressionMethods.Deflate});

            // 注册单例
            services.AddSingleton<BiliBiliHelp>();
            services.AddSingleton<DanmakuDao>();
            services.AddControllers().AddXmlSerializerFormatters();
            services.AddSignalR();

            // 权限
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                     {
                         options.LoginPath = "/api/admin/login";
                         options.LogoutPath = "/api/admin/logout";
                         options.Cookie.Name = "DCookie";
                         options.Cookie.MaxAge = TimeSpan.FromMinutes(config.GetAppSetting().Admin.MaxAge);
                     });

            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            // 转接头
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(DanmakuAllowSpecificOrigins, builder =>
                        builder.WithOrigins(config.GetAppSetting().WithOrigins)
                               .SetIsOriginAllowedToAllowWildcardSubdomains().WithMethods("GET", "POST", "OPTIONS")
                               .AllowAnyHeader());

                options.AddPolicy(LiveAllowSpecificOrigins, builder =>
                        builder.WithOrigins(config.GetAppSetting().LiveWithOrigins)
                               .SetIsOriginAllowedToAllowWildcardSubdomains().WithMethods("GET", "POST", "OPTIONS")
                               .AllowAnyHeader().AllowCredentials());
            });

            // SPA根目录
            services.AddSpaStaticFiles(opt => opt.RootPath = "wwwroot");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();


            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DanmakuContext>();
                context.Database.EnsureCreated();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(DanmakuAllowSpecificOrigins);
                endpoints.MapHub<LiveDanmaku>("api/dplayer/live").RequireCors(LiveAllowSpecificOrigins);
            });


            app.UseSpa(spa =>
            {
#if DEBUG
                spa.Options.SourcePath = "ClientApp";
                spa.UseVueCli();
#endif
            });
        }
    }
}
