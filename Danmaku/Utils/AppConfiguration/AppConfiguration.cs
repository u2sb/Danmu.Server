using Danmaku.Model;
using Microsoft.Extensions.Configuration;

namespace Danmaku.Utils.AppConfiguration
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly AppSettings _appConfiguration;

        public AppConfiguration(IConfiguration configuration)
        {
            _appConfiguration = new AppSettings(configuration);
        }

        public AppSettings GetAppSetting()
        {
            return _appConfiguration;
        }
    }
}