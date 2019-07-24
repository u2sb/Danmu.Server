using Danmaku.Model;
using Danmaku.Utils.BiliBili;
using Danmaku.Utils.PostgreSQL;
using Danmaku.Utils.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Danmaku
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IPsCon, PsCon>();
            services.AddSingleton<IDanmakuDao, DanmakuDao>();
            services.AddSingleton<IBilibiliHelp, BilibiliHelp>();
            services.AddSingleton<ILiveDanmaku, LiveDanmaku>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AppConfiguration appConfiguration = new AppConfiguration(Configuration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.AllowAnyOrigin().WithMethods("GET", "POST", "OPTIONS").AllowAnyHeader());
            }
            else
            {
                app.UseCors(builder =>
                    builder.WithOrigins(appConfiguration.WithOrigins).WithMethods("GET", "POST", "OPTIONS")
                        .AllowAnyHeader());
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            app.UseMvc();

            //TODO 临时启动
            var liveDanmaku = new LiveDanmaku(Configuration);
        }
    }
}