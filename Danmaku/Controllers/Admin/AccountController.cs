using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Danmaku.Model.WebResult;
using Danmaku.Utils.AppConfiguration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers.Admin
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Model.Admin _admin;

        public AccountController(IAppConfiguration configuration)
        {
            _admin = configuration.GetAppSetting().Admin;
        }

        [HttpGet("login")]
        public WebResult NoAuth()
        {
            return new WebResult(1)
            {
                Data = "没有权限"
            };
        }

        [HttpPost("login")]
        public async Task<WebResult> Login([FromBody] dynamic data)
        {
            var userName = data.TryGetProperty("name", out JsonElement a) ? a.GetString() : null;
            var password = data.TryGetProperty("password", out JsonElement b) ? b.GetString() : null;
            var returnUrl = data.TryGetProperty("url", out JsonElement c) ? c.GetString() : null;

            using var md5Hash = MD5.Create();
            var md5Data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(_admin.Password));
            var sBuilder = new StringBuilder();
            foreach (var t in md5Data)
                sBuilder.Append(t.ToString("x2"));

            if (userName == _admin.User && password == sBuilder.ToString())
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", "Member")
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme, "user", "role")));

                HttpContext.Response.Cookies.Append("ClientAuth", "0", new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromMinutes(_admin.MaxAge),
                    SameSite = SameSiteMode.Lax
                });


                if (Url.IsLocalUrl(returnUrl)) return new WebResult(0) {Data = new {url = returnUrl}};
                return new WebResult(0) {Data = new {url = "/"}};
            }

            return new WebResult(1) {Data = new {url = returnUrl}};
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
}
