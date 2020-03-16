using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine;
using Danmu.CommandLine.Utils;

namespace Danmu.CommandLine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(RunOptions)
                  .WithNotParsed(HandleParseError);
        }

        private static void RunOptions(Options opts)
        {
            if (opts.Version)
                Console.WriteLine((Assembly.GetEntryAssembly() ?? throw new InvalidOperationException())
                                 .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }
    }
}
