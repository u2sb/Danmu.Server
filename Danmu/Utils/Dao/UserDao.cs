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
        private readonly DanmuContext _con;

        public UserDao(DanmuContext con)
        {
            _con = con;
        }

        /// <summary>
        ///     验证用户名和密码是否匹配
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool VerPassword(string name, string password, out UserTable user)
        {
            user = null;
            var findUsers = _con.User.Where(e => e.Name.Equals(name));
            if (!findUsers.Any()) return false;

            foreach (var r in findUsers)
            {
                var salt = r.Salt;
                var vpd = Md5.GetMd5(password, salt);

                if (vpd.Equals(r.PassWord))
                {
                    user = r.ToSecurity();
                    return true;
                }
            }

            return false;
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
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password) ||
                !await CheckNameAsync(name)) return false;

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
        public bool ChangePassword(int id, string oldP, string newP)
        {
            if (string.IsNullOrWhiteSpace(oldP) || string.IsNullOrWhiteSpace(newP)) return false;
            var r0 = _con.User.Where(e => e.Id.Equals(id));
            if (!r0.Any()) return false;
            var salt = r0.Select(s => s.Salt).FirstOrDefault();
            var opd = Md5.GetMd5(oldP, salt);
            var r1 = r0.Where(e => e.PassWord.Equals(opd));
            if (!r1.Any()) return false;
            var salt1 = new RandomStringBuilder().Create(6);
            var r2 = r1.FirstOrDefault();
            if (r2 != null)
            {
                r2.Salt = salt1;
                r2.PassWord = Md5.GetMd5(newP, salt1);
                r2.UpdateTime = DateTime.UtcNow;
                _con.User.Update(r2);
            }

            return _con.SaveChanges() > 0;
        }

        /// <summary>
        ///     检查已使用用户名个数
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns>true表示用户名可用，false表示不可用</returns>
        private async Task<bool> CheckNameAsync(string name)
        {
            return !await _con.User.AsNoTracking().Where(e => e.Name.Equals(name)).AnyAsync();
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
            var user = _con.User.Where(e => e.Id.Equals(id));
            if (!await user.AnyAsync() || !await CheckNameAsync(name)) return false;
            var r = await user.FirstOrDefaultAsync();
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
            if (!await r.AnyAsync()) return false;

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
        public async Task<UserTable> UserInfoAsync(int? id)
        {
            if (id == null) return null;
            return await _con.User.AsNoTracking().Where(e => e.Id.Equals(id)).Select(s => s.ToSecurity())
                             .FirstOrDefaultAsync();
        }
    }
}
