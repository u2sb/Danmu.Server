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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using static Danmu.Utils.Global.VariableDictionary;

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
                opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                opt.JsonSerializerOptions.Converters.Add(new IPAddressConverter());
            });


            services.AddSignalR();
            services.AddRazorPages();
            services.AddServerSideBlazor();

            //数据库连接
            // ReSharper disable once ObjectCreationAsStatement
            services.AddDbContextPool<DanmuContext>(option => new DbContextBuild(config, option),
                    appSetting.DanmuSql.PoolSize);

            //Http请求
            services.AddHttpClient();
            services.AddHttpClient(Gzip).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {AutomaticDecompression = DecompressionMethods.GZip});
            services.AddHttpClient(Deflate).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {AutomaticDecompression = DecompressionMethods.Deflate});


            //Redis
            var redisConn = appSetting.Redis.ToConfigurationOptions();
            services.AddDataProtection()
                    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConn), "DataProtection-Keys");
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisConn;
                options.InstanceName = appSetting.Redis.InstanceName;
            });


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
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins(appSetting.WithOrigins)
                               .WithMethods("GET", "POST", "OPTIONS")
                               .AllowAnyHeader());

                options.AddPolicy(LiveAllowSpecificOrigins, builder =>
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins(appSetting.LiveWithOrigins)
                               .WithMethods("GET", "POST", "OPTIONS")
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
                         options.LoginPath = "/account/login";
                         options.LogoutPath = "/account/logout";
                         options.Cookie.Name = ".auth";
                         options.Cookie.MaxAge = TimeSpan.FromMinutes(appSetting.Admin.MaxAge);
                     });

            //Session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(appSetting.Admin.MaxAge);
                options.Cookie.MaxAge = TimeSpan.FromMinutes(appSetting.Admin.MaxAge);
                options.Cookie.Name = ".session";
                options.Cookie.HttpOnly = true;
            });


            //注入
            services.AddSingleton(s => config);

            services.AddScoped<UserDao>();
            services.AddScoped<DanmuDao>();
            services.AddScoped<VideoDao>();
            services.AddScoped<CacheDao>();
            services.AddScoped<BiliBiliHelp>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else app.UseExceptionHandler("/Error");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseForwardedHeaders();

            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LiveDanmu>("api/live/danmu").RequireCors(LiveAllowSpecificOrigins);
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
