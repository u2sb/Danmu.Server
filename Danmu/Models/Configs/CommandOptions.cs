using CommandLine;

namespace Danmu.Models.Configs
{
    public class CommandOptions
    {
        [Option("userName", Default = "Admin", HelpText = "设置超级管理员用户")]
        public string UserName { get; set; }

        [Option("password", HelpText = "设置超级管理员密码")]
        public string Password { get; set; }
    }
}
