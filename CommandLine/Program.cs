using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using Danmu.CommandLine.Utils;

namespace Danmu.CommandLine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //系统信息查询
            Start.StartCommandLine();
            
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(RunOptions)
                  .WithNotParsed(HandleParseError);
        }

        private static void RunOptions(Options opts)
        {
            if (opts.Version)
                Console.WriteLine((Assembly.GetEntryAssembly() ?? throw new InvalidOperationException())
                                 .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
            if (opts.Menu)
            {
                _ = Menu.MainMenu().Result;
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }
    }
}
