using Danmu.Model.Config;
using Danmu.Utils.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Model.DbContext
{
    public class BaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private protected readonly DanmuSql Sql;

        public BaseContext(DbContextOptions options) : base(options)
        {
            Sql = AppConfiguration.AppSettings.DanmuSql;
        }
    }
}
