using System;
using System.Linq;
using System.Threading.Tasks;
using Danmu.Model.Config;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Danmu.Utils.Common;
using Danmu.Utils.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Utils.Dao
{
    public class UserDao
    {
        private readonly Admin _admin;
        private readonly DanmuContext _con;

        public UserDao(AppConfiguration appConfiguration, DanmuContext con)
        {
            _admin = appConfiguration.GetAppSetting().Admin;
            _con = con;
        }

        /// <summary>
        ///     验证用户名和密码是否匹配
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<dynamic> VerPasswordAsync(string name, string password)
        {
            var r = _con.User.Where(e => e.Name.Equals(name));
            if (await r.CountAsync() == 0) return new {Succeed = false, Role = UserRole.GeneralUser};

            var salt = await r.Select(s => s.Salt).FirstOrDefaultAsync();
            var vpd = Md5.GetMd5(password, salt);
            var result =
                    await r.Where(e => e.PassWord.Equals(vpd))
                           .ToArrayAsync();
            return result.Length > 0
                    ? new
                    {
                        Succeed = true,
                        Role = result.Select(s => s.Role).FirstOrDefault(),
                        uid = result.Select(e => e.Id).FirstOrDefault()
                    }
                    : _admin.User.Equals(name) && Md5.IsThisMd5(password, _admin.Password)
                            ? new {Succeed = true, Role = UserRole.SuperAdmin, uid = 0}
                            : new {Succeed = false, Role = UserRole.GeneralUser, uid = 0};

            //TODO 这里需要修改，增加一种重置密码的方式，不再使用配置文件保存密码
        }

        /// <summary>
        ///     创建用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(string name, string password, string phoneNumber = "",
                                            string email = "", UserRole role = UserRole.GeneralUser)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password)) return false;
            var salt = new RandomStringBuilder().Create(6);
            var user = new UserTable
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Salt = salt,
                PassWord = Md5.GetMd5(password, salt),
                Role = role
            };

            await _con.User.AddAsync(user);
            return await _con.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     修改密码
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="oldP">旧密码</param>
        /// <param name="newP">新密码</param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(int id, string oldP, string newP)
        {
            if (string.IsNullOrWhiteSpace(oldP) || string.IsNullOrWhiteSpace(newP)) return false;
            var r0 = _con.User.Where(e => e.Id.Equals(id));
            if (await r0.CountAsync() == 0) return false;
            var salt = await r0.Select(s => s.Salt).FirstOrDefaultAsync();
            var ovpd = Md5.GetMd5(oldP, salt);
            var r1 = r0.Where(e => e.PassWord.Equals(ovpd));
            if (await r1.CountAsync() == 0) return false;
            var salt1 = new RandomStringBuilder().Create(6);
            var r2 = await r1.FirstOrDefaultAsync();
            r2.PassWord = Md5.GetMd5(newP, salt1);
            r2.Salt = salt1;
            r2.UpdateTime = DateTime.UtcNow;
            _con.User.Update(r2);
            return await _con.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     检查已使用用户名个数
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public async Task<int> CheckNameAsync(string name)
        {
            return await _con.User.AsNoTracking().Where(e => e.Name.Equals(name)).CountAsync();
        }

        /// <summary>
        ///     修改用户信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="name">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        public async Task<bool> ChangeUserInfoAsync(int id, string name = null, string email = null,
                                                    string phoneNumber = null)
        {
            if (await _con.User.Where(e => e.Name.Equals(name)).CountAsync() > 0 ||
                await _con.User.Where(e => e.Id.Equals(id)).CountAsync() == 0) return false;
            var r = await _con.User.Where(e => e.Id.Equals(id)).FirstOrDefaultAsync();
            r.Name = name ?? r.Name;
            r.Email = email ?? r.Email;
            r.PhoneNumber = phoneNumber ?? r.PhoneNumber;
            r.UpdateTime = DateTime.UtcNow;
            _con.User.Update(r);
            return await _con.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     修改用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> ChangeUserRoleAsync(int id, UserRole role)
        {
            var r = _con.User.Where(e => e.Id.Equals(id));
            if (await r.CountAsync() == 0) return false;

            var r1 = await r.FirstOrDefaultAsync();
            r1.Role = role;
            _con.User.Update(r1);
            return await _con.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     查看某用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserTable> UserInfoAsync(int id)
        {
            return await _con.User.AsNoTracking().Where(e => e.Id.Equals(id)).Select(s => new UserTable
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                CreateTime = s.CreateTime,
                UpdateTime = s.UpdateTime,
                Role = s.Role
            }).FirstOrDefaultAsync();
        }
    }
}
