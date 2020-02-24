using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Danmaku.Model;
using Danmaku.Utils.AppConfiguration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Danmaku.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Admin _admin;

        public AccountController(IAppConfiguration configuration)
        {
            _admin = configuration.GetAppSetting().Admin;
        }

        [HttpPost("login")]
        public async Task<dynamic> Login([FromBody] dynamic data)
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

                if (Url.IsLocalUrl(returnUrl)) return new {code = 0, url = returnUrl};
                return new {code = 0, url = "/"};
            }

            return new {code = 1, url = returnUrl};
        }

        [HttpGet("logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync();
            Response.Headers.Add("Location", "/");
            Response.StatusCode = 302;
        }
    }
}
