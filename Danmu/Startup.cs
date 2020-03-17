using System;
using System.Net;
using System.Net.Http;
using Danmu.Model.Converter;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Danmu.Utils;
using Danmu.Utils.BiliBili;
using Danmu.Utils.Configuration;
using Danmu.Utils.Dao;
using Danmu.Utils.LiveDanmu;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Danmu.Utils.Global.VariableDictionary;
#if DEBUG
using VueCliMiddleware;

#endif

namespace Danmu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AppSetting配置文件
            var config = new AppConfiguration(Configuration);
            var appSetting = config.GetAppSetting();


            //自定义序列化程序
            services.AddControllers().AddXmlSerializerFormatters().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new IPAddressConverter());
            });
            services.AddSignalR();

            //数据库连接
            // ReSharper disable once ObjectCreationAsStatement
            services.AddDbContextPool<DanmuContext>(option => new DbContextBuild(config, option),
                    appSetting.DanmuSql.PoolSize);


            //Http请求
            services.AddHttpClient(Gzip).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {AutomaticDecompression = DecompressionMethods.GZip});
            services.AddHttpClient(Deflate).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {AutomaticDecompression = DecompressionMethods.Deflate});

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
                        builder.WithOrigins(appSetting.WithOrigins)
                               .SetIsOriginAllowedToAllowWildcardSubdomains().WithMethods("GET", "POST", "OPTIONS")
                               .AllowAnyHeader());

                options.AddPolicy(LiveAllowSpecificOrigins, builder =>
                        builder.WithOrigins(appSetting.LiveWithOrigins)
                               .SetIsOriginAllowedToAllowWildcardSubdomains().WithMethods("GET", "POST", "OPTIONS")
                               .AllowAnyHeader().AllowCredentials());

                options.AddPolicy(AdminAllowSpecificOrigins, builder =>
                        builder.WithOrigins(appSetting.AdminWithOrigins)
                               .WithMethods("GET", "POST", "OPTIONS"));
            });

            //权限
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AdminRolePolicy,
                        policy => policy.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin)));
                options.AddPolicy(GeneralUserRolePolicy,
                        policy => policy.RequireRole(nameof(UserRole.GeneralUser), nameof(UserRole.SuperAdmin),
                                nameof(UserRole.Admin)));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                     {
                         options.ReturnUrlParameter = "url";
                         options.AccessDeniedPath = "/api/admin/noAuth";
                         options.LoginPath = "/api/admin/login";
                         options.LogoutPath = "/api/admin/logout";
                         options.Cookie.Name = "DCookie";
                         options.Cookie.MaxAge = TimeSpan.FromMinutes(appSetting.Admin.MaxAge);
                     });

            // SPA根目录
            services.AddSpaStaticFiles(opt => opt.RootPath = "wwwroot");


            //注入
            services.AddSingleton(s => config);

            services.AddScoped<UserDao>();
            services.AddScoped<DanmuDao>();
            services.AddScoped<VideoDao>();
            services.AddScoped<HttpClientCacheDao>();
            services.AddScoped<BiliBiliHelp>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSpaStaticFiles();
            app.UseDefaultFiles();

            app.UseForwardedHeaders();

            app.UseCookiePolicy();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LiveDanmu>("api/live/danmu").RequireCors(LiveAllowSpecificOrigins);
            });

            app.UseSpa(spa =>
            {
#if DEBUG
                spa.Options.SourcePath = "ClientApp";
                spa.UseVueCli("serve",18081);
#endif
            });
        }
    }
}
