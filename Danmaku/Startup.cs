using Danmaku.Utils.BiliBili;
using Danmaku.Utils.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

#if DEBUG
            services.AddSingleton<IMySqlCon, MySqlConDev>();
#else
            services.AddSingleton<IMySqlCon, MySqlCon>();
#endif

            services.AddSingleton<IDanmakuDao, DanmakuDao>();
            services.AddSingleton<IBilibiliHelp, BilibiliHelp>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.AllowAnyOrigin().WithMethods("GET", "POST", "OPTIONS").AllowAnyHeader());
            }
            else
            {
                app.UseCors(builder =>
                    builder.WithOrigins(Configuration["WithOrigins"].Split(",")).WithMethods("GET", "POST", "OPTIONS").AllowAnyHeader());
            }
            
            app.UseMvc();
        }
    }
}