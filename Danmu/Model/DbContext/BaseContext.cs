using Danmu.Model.Config;
using Microsoft.EntityFrameworkCore;

namespace Danmu.Model.DbContext
{
    public class BaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private protected readonly DanmuSql Sql;

        public BaseContext(DbContextOptions options) : base(options)
        {
            Sql = Program.AppSettings.DanmuSql;
        }
    }
}
