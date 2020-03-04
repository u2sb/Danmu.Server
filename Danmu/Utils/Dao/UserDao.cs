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
        private readonly DanmuContext _context;

        public UserDao(AppConfiguration appConfiguration, DanmuContext context)
        {
            _admin = appConfiguration.GetAppSetting().Admin;
            _context = context;
        }

        public async Task<dynamic> VerPasswordAsync(string name, string password)
        {
            var r = _context.User.Where(e => e.Name.Equals(name));
            if (await r.CountAsync() == 0) return new {Succeed = false, Role = UserRole.GeneralUser};

            var salt = await r.Select(s => s.Salt).FirstOrDefaultAsync();
            var vpd = Md5.GetMd5(password, salt);
            var result =
                    await r.Where(e => e.PassWord.Equals(vpd))
                           .ToArrayAsync();
            return result.Length > 0
                    ? new {Succeed = true, Role = result.Select(s => s.Role).FirstOrDefault()}
                    : _admin.User.Equals(name) && Md5.IsThisMd5(password, _admin.Password)
                            ? new {Succeed = true, Role = UserRole.SuperAdmin}
                            : new {Succeed = false, Role = UserRole.GeneralUser};

            //TODO 这里需要修改，增加一种重置密码的方式，不再使用配置文件保存密码
        }
    }
}