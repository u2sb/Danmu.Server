using System;
using System.Threading.Tasks;
using Danmu.Models.Danmus.DataTables;
using LiteDB;
using NETCore.Encrypt.Extensions;
using RandomGen;

namespace Danmu.Utils.Dao.Danmu
{
    public class UserDao
    {
        private readonly ILiteCollection<UserTable> _userTable;

        public UserDao(LiteDbContext con)
        {
            _userTable = con.Database.GetCollection<UserTable>();
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
            var findUsers = _userTable.Query()
                .Where(e => e.Name.Equals(name));
            if (findUsers.Count() == 0) return false;

            foreach (var r in findUsers.ToArray())
            {
                var salt = r.Salt;
                var vpd = password.HMACMD5(salt);

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
                !CheckName(name)) return false;

            var salt = Gen.Random.Text.Length(6)();
            var user = new UserTable
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Salt = salt,
                PassWord = password.HMACMD5(salt),
                Role = role
            };

            return await Task.Run(() => _userTable.Insert(user));
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
            var r0 = _userTable.Query()
                .Where(e => e.Id.Equals(id));
            if (r0.Count() == 0) return false;
            var salt = r0.Select(s => s.Salt).FirstOrDefault();
            var opd = oldP.HMACMD5(salt);
            var r1 = r0.Where(e => e.PassWord.Equals(opd));
            if (r1.Count() == 0) return false;
            var salt1 = Gen.Random.Text.Length(6)();
            var r2 = r1.FirstOrDefault();
            if (r2 != null)
            {
                r2.Salt = salt1;
                r2.PassWord = newP.HMACMD5(salt1);
                r2.UpdateTime = DateTime.UtcNow;
                return _userTable.Update(r2);
            }

            return false;
        }

        /// <summary>
        ///     检查已使用用户名个数
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns>true表示用户名可用，false表示不可用</returns>
        private bool CheckName(string name)
        {
            return _userTable.Query()
                .Where(e => e.Name.Equals(name)).Count() == 0;
        }

        /// <summary>
        ///     修改用户信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="name">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        public bool ChangeUserInfo(int id, string name = null, string email = null,
            string phoneNumber = null)
        {
            var user = _userTable.Query()
                .Where(e => e.Id.Equals(id));
            if (user.Count() == 0 || !CheckName(name)) return false;
            var r = user.FirstOrDefault();
            r.Name = name ?? r.Name;
            r.Email = email ?? r.Email;
            r.PhoneNumber = phoneNumber ?? r.PhoneNumber;
            r.UpdateTime = DateTime.UtcNow;
            return _userTable.Update(r);
        }

        /// <summary>
        ///     修改用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool ChangeUserRole(int id, UserRole role)
        {
            var r = _userTable.Query()
                .Where(e => e.Id.Equals(id));
            if (r.Count() == 0) return false;

            var r1 = r.FirstOrDefault();
            r1.Role = role;
            return _userTable.Update(r1);
        }

        /// <summary>
        ///     查看某用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserTable UserInfo(int? id)
        {
            if (id == null) return null;
            return _userTable.Query()
                .Where(e => e.Id.Equals(id))
                .Select(s => s.ToSecurity())
                .FirstOrDefault();
        }
    }
}
