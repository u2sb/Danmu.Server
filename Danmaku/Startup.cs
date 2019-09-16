using Danmaku.Model;
using Danmaku.Utils.BiliBili;
using Danmaku.Utils.Dao;
using Danmaku.Utils.LiveDanmaku;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Danmaku.Model.AppConfiguration;

namespace Danmaku
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Config = new AppConfiguration(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DanmakuContext>();
            services.AddSingleton<IBiliBiliHelp, BiliBiliHelp>();
            services.AddSingleton<IDanmakuDao, DanmakuDao>();
            services.AddControllers();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DanmakuContext>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.WithOrigins("http://localhost","http://localhost:*").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            }
            else
            {
                app.UseCors(builder =>
                    builder.WithOrigins(Config.WithOrigins).WithMethods("GET", "POST", "OPTIONS")
                        .AllowAnyHeader().AllowCredentials());
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LiveDanmaku>("dplayer/live");
            });
        }
    }
}