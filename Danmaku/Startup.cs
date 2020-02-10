using System;
using System.Net;
using System.Net.Http;
using Danmaku.Model;
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

			services.AddSingleton<IBiliBiliHelp, BiliBiliHelp>();
			services.AddSingleton<IDanmakuDao, DanmakuDao>();
			services.AddControllersWithViews();
			services.AddSignalR();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/account/login";
					options.LogoutPath = "/account/logout";
					options.Cookie.Name = "DCookie";
					options.Cookie.MaxAge = TimeSpan.FromHours(1);
				});

			services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

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
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

			app.UseCors(DanmakuAllowSpecificOrigins);

			app.UseStaticFiles();

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<DanmakuContext>();
				context.Database.EnsureCreated();
			}

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapDefaultControllerRoute();
				endpoints.MapHub<LiveDanmaku>("api/dplayer/live").RequireCors(LiveAllowSpecificOrigins);
			});
		}
	}
}