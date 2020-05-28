using System.Threading.Tasks;
using Danmu.Model.DataTable;
using Danmu.Model.WebResult;
using Danmu.Utils.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        ///     创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.SuperAdmin))]
        [HttpPost("insert")]
        public async Task<WebResult> InsertUser([FromBody] UserTable user)
        {
            var uid = HttpContext?.Session.GetInt32("uid");
            var role = (await _userDao.UserInfoAsync(uid)).Role;
            var result = false;
            if (role.Equals(UserRole.SuperAdmin))
                result = await _userDao.InsertAsync(user.Name, user.PassWord, user.PhoneNumber, user.Email, user.Role);

            return new WebResult(result ? 0 : 1);
        }

        /// <summary>
        ///     修改密码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("change" + "password")]
        public WebResult ChangePassword([FromBody] ChangePasswordData data)
        {
            var sUid = HttpContext?.Session.GetInt32("uid");
            var result = false;
            if (sUid == data.Uid) result = _userDao.ChangePassword(data.Uid, data.OldP, data.NewP);
            return new WebResult(result ? 0 : 1);
        }

        /// <summary>
        ///     修改用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("change" + "info")]
        public async Task<WebResult> ChangeUserInfo([FromBody] UserTable user)
        {
            var uid = HttpContext?.Session.GetInt32("uid");
            var result = false;
            if (uid == user.Id || (await _userDao.UserInfoAsync(uid)).Role == UserRole.SuperAdmin)
                result = await _userDao.ChangeUserInfoAsync(user.Id, user.Name, user.Email, user.PhoneNumber);
            return new WebResult(result ? 0 : 1);
        }

        /// <summary>
        ///     修改用户角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.SuperAdmin))]
        [HttpPost("change" + "role")]
        public async Task<WebResult> ChangeUserRole([FromBody] UserTable user)
        {
            var uid = HttpContext?.Session.GetInt32("uid");
            var role = (await _userDao.UserInfoAsync(uid)).Role;
            var result = false;
            if (role == UserRole.SuperAdmin)
                result = await _userDao.ChangeUserRoleAsync(user.Id, user.Role);
            return new WebResult(result ? 0 : 1);
        }

        /// <summary>
        ///     查看用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<WebResult<UserTable>> UserInfo(int uid)
        {
            var sUid = HttpContext?.Session.GetInt32("uid");
            var result = new UserTable();
            if (sUid == uid || (await _userDao.UserInfoAsync(sUid)).Role == UserRole.SuperAdmin)
                result = await _userDao.UserInfoAsync(uid);
            return new WebResult<UserTable>(result);
        }

        /// <summary>
        ///     修改密码数据
        /// </summary>
        public class ChangePasswordData
        {
            public int Uid { get; set; }
            public string OldP { get; set; }
            public string NewP { get; set; }
        }
    }
}
