using System;

namespace Danmu.CommandLine.Utils
{
    public class Start
    {
        public static void StartCommandLine()
        {
            Console.WriteLine(Environment.Is64BitProcess);
        }
    }
}