using DanMu.Models.Settings;
using DanMu.Utils.BiliBili;
using DanMu.Utils.Caching;
using DanMu.Utils.Program;
using MemoryPack.AspNetCoreMvcFormatter;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using RestSharp;

var builder = WebApplication.CreateSlimBuilder(args);

var appSettings = builder.Configuration.Get<AppSettings>()!;

builder.WebHost.ConfigureKestrel((b, options) =>
{
  var unixSocket = appSettings?.UnixSocket;
  var port = appSettings?.Port ?? 0;
  if (port > 0)
    options.ListenLocalhost(port, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3; });

  if (!string.IsNullOrWhiteSpace(unixSocket))
    options.ListenUnixSocket(unixSocket, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
});

var services = builder.Services;

if (!Directory.Exists(appSettings.DataBase.Directory))
  Directory.CreateDirectory(appSettings.DataBase.Directory);

services.AddControllers(options =>
{
  options.InputFormatters.Add(new MemoryPackInputFormatter());
  options.OutputFormatters.Add(new MemoryPackOutputFormatter(true));

  options.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
  options.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
}).AddXmlSerializerFormatters();

services.Configure<ForwardedHeadersOptions>(options =>
{
  options.ForwardedHeaders =
    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

services.AddCors(options =>
{
  options.AddDefaultPolicy(b => b
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .WithOrigins(appSettings.WithOrigins.ToArray())
    .WithMethods("GET", "POST", "OPTIONS")
    .AllowAnyHeader());
});

services.AddAuthentication();

services.AddSingleton(appSettings);
services.AddSingleton<RestClient>();
services.AddSingleton<CachingContext>();
services.AddSingleton<BiliBiliCaching>();
services.AddSingleton<SbLife>();
services.AddScoped<BiliBiliHelp>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseForwardedHeaders();

app.UseAuthentication();

app.MapControllers();

var serviceScope = app.Services.CreateScope();
var s = serviceScope.ServiceProvider;

var life = app.Lifetime;
var sbLife = s.GetService<SbLife>();
sbLife?.Register(life);

app.Run();