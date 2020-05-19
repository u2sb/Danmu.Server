using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Danmu.Controllers.Base;
using Danmu.Model.Danmu.Account;
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
        public async Task<WebResult> Login([FromBody] LoginData data)
        {
            if (_userDao.VerPassword(data.UserName, data.Password, out var user))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", user.Name),
                    new Claim("role", user.Role.ToString())
                };

                var sign = HttpContext.SignInAsync(
                        new ClaimsPrincipal(
                                new ClaimsIdentity(
                                        claims,
                                        CookieAuthenticationDefaults.AuthenticationScheme, "user", "role"
                                )));

                HttpContext.Session.SetInt32("uid", user.Id);

                HttpContext.Response.Cookies.Append(".client", user.Role.ToString(), new CookieOptions
                {
                    HttpOnly = false,
                    MaxAge = TimeSpan.FromMinutes(_admin.MaxAge),
                    SameSite = SameSiteMode.Lax
                });


                if (Url.IsLocalUrl(data.Url)) return new WebResult(0) {Data = new {data.Url}};
                await sign;
                return new WebResult(0) {Data = new {Url = "/"}};
            }

            return new WebResult(1) {Data = new {ReturnUrl = ""}};
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
