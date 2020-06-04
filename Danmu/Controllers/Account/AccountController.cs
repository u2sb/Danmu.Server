using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Danmu.Model.Danmu.Account;
using Danmu.Model.WebResult;
using Danmu.Utils.Common;
using Danmu.Utils.Configuration;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Danmu.Controllers.Account
{
    [Route("/account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly Model.Config.Admin _admin;
        private readonly UserDao _userDao;

        public AccountController(AppConfiguration configuration, UserDao userDao)
        {
            _admin = configuration.GetAppSetting().Admin;
            _userDao = userDao;
        }

        [HttpGet("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost("loginApi")]
        public async Task<WebResult> Login([FromBody] LoginData data)
        {
            if (_userDao.VerPassword(data.UserName, data.Password, out var user))
            {
                Logout();

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

                if (Url.IsLocalUrl(data.Url)) return new WebResult(0) {Data = new {data.Url}};
                await sign;
                return new WebResult(0) {Data = new {Url = "/"}};
            }

            return new WebResult(1) {Data = new {Url = ""}};
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            var result = await Login(new LoginData
                    {UserName = userName, Password = Md5.GetMd5(password), Url = returnUrl});
            if (result.Code == 0) return Redirect(result.Data.Url);
            return View();
        }

        [HttpGet("logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync();
            Response.Redirect("/");
        }
    }
}
