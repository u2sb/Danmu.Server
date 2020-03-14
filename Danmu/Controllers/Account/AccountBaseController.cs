using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Configuration;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Account
{
    [Route("/api/admin")]
    [AllowAnonymous]
    public class AccountBaseController : AdminBaseController
    {
        private readonly Model.Config.Admin _admin;
        private readonly UserDao _userDao;

        public AccountBaseController(AppConfiguration configuration, UserDao userDao, DanmuDao danmuDao,
                                     VideoDao videoDao) : base(danmuDao, videoDao)
        {
            _admin = configuration.GetAppSetting().Admin;
            _userDao = userDao;
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
            var userName = data.TryGetProperty("name", out JsonElement a) ? a.GetString() : null;
            var password = data.TryGetProperty("password", out JsonElement b) ? b.GetString() : null;
            var url = data.TryGetProperty("url", out JsonElement c) ? c.GetString() : null;

            var r = await _userDao.VerPasswordAsync(userName, password);
            if (r.Succeed)
            {
                UserRole role = r.Role;
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", role.ToString())
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme, "user", "role")));

                HttpContext.Response.Cookies.Append("ClientAuth", role.ToString(), new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromMinutes(_admin.MaxAge),
                    SameSite = SameSiteMode.Lax
                });


                if (Url.IsLocalUrl(url)) return new WebResult(0) {Data = new {url = url, uid = r.uid}};
                return new WebResult(0) {Data = new {url = "/", uid = r.uid } };
            }

            return new WebResult(1) {Data = new {url = url}};
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
