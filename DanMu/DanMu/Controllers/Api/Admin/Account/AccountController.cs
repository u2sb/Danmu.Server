using System.Security.Claims;
using System.Text.Json;

using DanMu.Models.DataTables.User;
using DanMu.Models.Settings;
using DanMu.Models.WebResults;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DanMu.Controllers.Api.Admin.Account;

[ApiController]
[Route("/api/admin/account/")]
public class AccountController : ControllerBase
{
    private readonly AppSettings _appSettings;

    public AccountController(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    [HttpGet("login")]
    [HttpGet("noAuth")]
    public WebResult NoAuth()
    {
        return new WebResult(401)
        {
            Data = new
            {
                desc = "没有权限"
            }
        };
    }

    [HttpPost("login")]
    public async Task<WebResult> Login([FromBody] dynamic data)
    {
        var userName = data.TryGetProperty("username", out JsonElement a) ? a.GetString() : null;
        var password = data.TryGetProperty("password", out JsonElement b) ? b.GetString() : null;
        var url = data.TryGetProperty("path", out JsonElement c) ? c.GetString() : null;

        if (userName != null && password != null)
        {
            var r = _appSettings.Admins.Any(w => w.UserName == userName && w.Password == password);


            if (r)
            {
                var role = UserRole.Admin;
                var claims = new List<Claim>
                {
                    new("user", userName),
                    new("role", role.ToString())
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme, "user", "role")));

                HttpContext.Response.Cookies.Append("ClientAuth", role.ToString(), new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromMinutes(120),
                    SameSite = SameSiteMode.Lax
                });


                if (Url.IsLocalUrl(url)) return new WebResult(0) { Data = new { url } };
                return new WebResult(0) { Data = new { url = "/" } };
            }
        }

        return new WebResult(1) { Data = new { url } };
    }


    [HttpGet("logout")]
    public void Logout()
    {
        HttpContext.SignOutAsync();
        HttpContext.Response.Cookies.Delete("ClientAuth");
        Response.Headers.Add("Location", "/");
        Response.StatusCode = 302;
    }
}