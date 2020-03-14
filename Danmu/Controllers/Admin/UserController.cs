using System.Text.Json;
using System.Threading.Tasks;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static Danmu.Utils.Global.VariableDictionary;

namespace Danmu.Controllers.Admin
{
    [ApiController]
    [EnableCors(AdminAllowSpecificOrigins)]
    [Authorize(Policy = AdminRolePolicy)]
    [Route("/api/admin/user/")]
    public class UserController : ControllerBase
    {
        private readonly UserDao _userDao;

        public UserController(UserDao userDao)
        {
            _userDao = userDao;
        }

        //TODO: 这里权限有问题，暂时关闭，待解决

        // /// <summary>
        // ///     创建用户
        // /// </summary>
        // /// <param name="user"></param>
        // /// <returns></returns>
        // [Authorize(Roles = nameof(UserRole.SuperAdmin))]
        // [HttpPost("insert")]
        // public async Task<WebResult> InsertUser([FromBody] UserTable user)
        // {
        //     var result = await _userDao.InsertAsync(user.Name, user.PassWord, user.PhoneNumber, user.Email, user.Role);
        //     return new WebResult(result ? 0 : 1);
        // }

        /// <summary>
        ///     修改密码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("change" + "password")]
        public async Task<WebResult> ChangePassword([FromBody] dynamic data)
        {
            var uid = data.TryGetProperty("uid", out JsonElement a) ? a.GetInt32() : 0;
            var oldP = data.TryGetProperty("oldP", out JsonElement b) ? b.GetString() : null;
            var newP = data.TryGetProperty("newP", out JsonElement c) ? c.GetString() : null;

            var result = await _userDao.ChangePasswordAsync(uid, oldP, newP);
            return new WebResult(result ? 0 : 1);
        }

        /// <summary>
        ///     修改用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("change" + "info")]
        public async Task<WebResult> ChangeUserInfo([FromBody] UserTable user)
        {
            var result = await _userDao.ChangeUserInfoAsync(user.Id, user.Name, user.Email, user.PhoneNumber);
            return new WebResult(result ? 0 : 1);
        }
        //
        // /// <summary>
        // ///     修改用户角色
        // /// </summary>
        // /// <param name="user"></param>
        // /// <returns></returns>
        // [Authorize(Roles = nameof(UserRole.SuperAdmin))]
        // [HttpPost("change" + "role")]
        // public async Task<WebResult> ChangeUserRole([FromBody] UserTable user)
        // {
        //     var result = await _userDao.ChangeUserRoleAsync(user.Id, user.Role);
        //     return new WebResult(result ? 0 : 1);
        // }

        /// <summary>
        ///     查看用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<WebResult<UserTable>> UserInfo(int uid)
        {
            var result = await _userDao.UserInfoAsync(uid);
            return new WebResult<UserTable>(result);
        }
    }
}
