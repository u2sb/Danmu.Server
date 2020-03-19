using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Shell;
using ShellProgressBar;
using static System.Console;
using static Danmu.CommandLine.Utils.GlobalConstant;
using static Danmu.CommandLine.Utils.Utils;

namespace Danmu.CommandLine.Utils
{
    public static class Menu
    {
        public static async Task<int> MainMenu()
        {
            var menu = LoadMenuAsync("MainMenu.txt");
            Utils.Clear();
            Write(await menu);
            var key = ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.A:
                    await A();
                    break;
                case ConsoleKey.X:
                    X();
                    break;
                default:
                    await MainMenu();
                    break;
            }

            return 0;
        }

        /// <summary>
        /// 查询最新版本
        /// </summary>
        /// <returns></returns>
        private static async Task A()
        {
            Utils.Clear();
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593',
                DisplayTimeInRealTime = false,
                ProgressBarOnBottom = true
            };
            using var pbar = new ProgressBar(10, "正在查询", options);
            pbar.Tick(1);
            pbar.Tick("正在查询相关数据"); 
            var releases = GitHub.Repository.Release.GetAll(GithubUserName, GithubRepoName).Result;
            var latest = releases[0];
            pbar.Tick(8);
            pbar.Tick("查询完成"); 
            Thread.Sleep(200);
            pbar.Tick(10);
            Thread.Sleep(100);
            WriteLine("\r\n  \r\n ");
            WriteLine($"最新版本：{latest.TagName}");
            WriteLine("\r\n按任意键继续");
            ReadKey();
            await MainMenu();
        }

        private static void X()
        {
            Utils.Clear();
            WriteLine("再见");
        }
    }
}