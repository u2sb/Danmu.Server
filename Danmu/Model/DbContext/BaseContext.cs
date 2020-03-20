using System.IO;
using System.Text;
using System.Text.Json;
using Danmu.Model.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Danmu.Model.DbContext
{
    public class BaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private protected readonly DanmuSql Sql;

        public BaseContext(DbContextOptions options) : base(options)
        {
            using var sr = new StreamReader("appsettings.json", Encoding.UTF8);
            var s = sr.ReadToEnd();
            var settings = JsonSerializer.Deserialize<AppSettings>(s);
            Sql = settings.DanmuSql;
        }
    }
}
