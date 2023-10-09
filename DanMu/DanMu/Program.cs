using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using DanMu.Models.Settings;
using DanMu.Utils.BiliBili;
using DanMu.Utils.Caching;
using DanMu.Utils.Program;
using Microsoft.AspNetCore.HttpOverrides;
using RestSharp;
using WebApiProtobufFormatter;

var builder = WebApplication.CreateSlimBuilder(args);

var appSettings = builder.Configuration.Get<AppSettings>()!;

builder.WebHost.ConfigureKestrel((b, options) =>
{
  var unixSocket = appSettings?.UnixSocket;
  var port = appSettings?.Port ?? 0;
  if (port > 0)
    options.ListenLocalhost(port);

  if (!string.IsNullOrWhiteSpace(unixSocket))
    options.ListenUnixSocket(unixSocket);
});


var services = builder.Services;

if (!Directory.Exists(appSettings.DataBase.Directory))
  Directory.CreateDirectory(appSettings.DataBase.Directory);

services.AddControllers().AddProtobufFormatters(opt =>
{
  opt.OutputFormatterOptions.ContentTypeDefault = "application/x-protobuf";
}).AddJsonOptions(opt =>
{
  opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
  opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
  opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
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
app.UseRouting();

app.MapControllers();

var serviceScope = app.Services.CreateScope();
var s = serviceScope.ServiceProvider;

var life = app.Lifetime;
var sbLife = s.GetService<SbLife>();
sbLife?.Register(life);

app.Run();