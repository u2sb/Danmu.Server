using System.Linq;
using Danmu.Model.Config;
using Danmu.Model.DataTable;
using Danmu.Model.DbContext;
using Danmu.Utils.Common;

namespace Danmu.Utils.Dao
{
    public class DbInitializer
    {
        public static void Initialize(DanmuContext context, AppSettings appSettings)
        {
            context.Database.EnsureCreated();

            if (!context.User.Any())
            {
                var admin = appSettings.Admin;
                var salt = new RandomStringBuilder().Create(6);

                var user = new UserTable
                {
                    Name = admin.User,
                    Salt = salt,
                    PassWord = Md5.GetMd5(Md5.GetMd5(admin.Password), salt),
                    Role = UserRole.SuperAdmin
                };

                context.User.AddAsync(user);
                context.SaveChanges();
            }
        }
    }
}
