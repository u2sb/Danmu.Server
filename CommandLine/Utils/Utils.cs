using System;
using System.IO;
using System.Threading.Tasks;
using Medallion.Shell;
using Octokit;

namespace Danmu.CommandLine.Utils
{
    public static class Utils
    {
        public static GitHubClient GitHub
            => new GitHubClient(new ProductHeaderValue("Awesome-Octocat-App"));

        public static async Task<string> LoadMenuAsync(string name)
        {
            return await File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}/Scripts/Menu/{name}");
        }

        public static void Clear()
        {
            var a = Command.Run("clear").StandardOutput.ReadToEnd();
            Console.Write(a);
        }
    }
}