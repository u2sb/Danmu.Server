using System.Text.Json;
using DanMu.Models.Converters;
using DanMu.Models.Settings;
using DanMu.Utils.BiliBiliHelp;
using DanMu.Utils.Cache;
using DanMu.Utils.Dao.DbContext;
using EasyCaching.LiteDB;
using LiteDB;
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
    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opt.JsonSerializerOptions.Converters.Add(new IpAddressConverter());
});

// 配置数据库
services.AddDbContextPool<DanMuDbContext>(options => new DbContextBuild(appSettings.DataBase, options),
    appSettings.DataBase.PoolSize);


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

// 注入
services.AddSingleton<AppSettings>();
services.AddSingleton(new RestClient(new HttpClient()));
services.AddScoped<BiliBiliCache>();
services.AddScoped<BiliBiliHelp>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseForwardedHeaders();
app.UseCors();

app.MapControllers();

app.Run();