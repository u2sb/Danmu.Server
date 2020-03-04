using Danmu.Model.Config;
using Microsoft.Extensions.Configuration;

namespace Danmu.Utils.Configuration
{
    public class AppConfiguration
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