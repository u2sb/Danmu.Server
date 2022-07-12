using System.Text.Encodings.Web;
using System.Text.Json;
using DanMu.Models.Converters;
using DanMu.Models.DataTables.User;
using DanMu.Models.Settings;
using DanMu.Utils.Authorization;
using DanMu.Utils.BiliBiliHelp;
using DanMu.Utils.Cache;
using DanMu.Utils.Dao.Danmu;
using DanMu.Utils.Dao.DbContext;
using EasyCaching.LiteDB;
using LiteDB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using RestSharp;
using static DanMu.Models.Settings.ConstantTable;

var builder = WebApplication.CreateBuilder(args);

// 配置文件
var appSettings = builder.Configuration.Get<AppSettings>();

// 创建数据库目录
if (!Directory.Exists(appSettings.DataBase.Directory)) Directory.CreateDirectory(appSettings.DataBase.Directory);

// Add services to the container.
var services = builder.Services;

// 配置自定义序列化程序
// https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0
services.AddControllers().AddXmlSerializerFormatters().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opt.JsonSerializerOptions.Converters.Add(new IpAddressConverter());
});


// 配置弹幕缓存数据库
// https://github.com/dotnetcore/EasyCaching
services.AddEasyCaching(options =>
{
    //use memory cache
    options.UseLiteDB(config =>
    {
        config.DBConfig = new LiteDBDBOptions
        {
            ConnectionType = ConnectionType.Direct,
            FilePath = appSettings.DataBase.Directory,
            FileName = appSettings.DataBase.DanMuCachingDb
        };
    }, BiliBiliDanMuCacheLiteDbName);
});

// 配置转接头，代理
// https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0#use-a-reverse-proxy-server
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

//配置跨域
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithMethods("GET", "POST", "OPTIONS").SetIsOriginAllowedToAllowWildcardSubdomains()
            .WithOrigins(appSettings.WithOrigins).AllowAnyHeader());
});


//权限
services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
services.AddAuthorization(options =>
{
    options.AddPolicy(AdminRolePolicy,
        policy => policy.RequireRole(nameof(UserRole.SuperAdmin), nameof(UserRole.Admin)));
});
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ReturnUrlParameter = "url";
        options.AccessDeniedPath = "/api/admin/account/noAuth";
        options.LoginPath = "/api/admin/account/login";
        options.LogoutPath = "/api/admin/account/logout";
        options.Cookie.Name = "DCookie";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(120);
    });

// 注入
services.AddSingleton<AppSettings>();
services.AddSingleton<DanMuDbContext>();
services.AddSingleton(new RestClient(new HttpClient()));
services.AddScoped<BiliBiliCache>();
services.AddScoped<BiliBiliHelp>();
services.AddScoped<DanmuTableDao>();

services.AddSpaStaticFiles(opt => { opt.RootPath = "wwwroot"; });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseForwardedHeaders();

app.UseCookiePolicy();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
    spa.Options.DefaultPage = "/index.html";
});

app.Run();