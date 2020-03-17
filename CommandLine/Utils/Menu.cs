
using System;

namespace Danmu.CommandLine.Utils
{
    public static class Menu
    {
        public static void MainMenu()
        {
            var menu = Properties.Resources.MainMenu;
            Console.Write(menu);
            var key = Console.ReadKey();
            Console.WriteLine(key.Key.Equals(ConsoleKey.A));

            switch (key.Key)
            {
                case ConsoleKey.X:
                    X();
                    break;
                default:
                    MainMenu();
                    break;
            }
        }

        public static void A()
        {
            Console.ReadKey();
            MainMenu();
        }

        public static void X()
        {
            Console.WriteLine("再见");
        }
    }
}
