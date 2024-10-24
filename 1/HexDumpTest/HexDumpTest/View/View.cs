using System;

namespace HexDumpApp
{
    public class View : IView
    {
        public string GetFilePath()
        {
            Console.Write("Введите путь к файлу: ");
            return Console.ReadLine();
        }

        public void ShowHexDump(string hexDump)
        {
            Console.WriteLine("Hex Dump:");
            Console.WriteLine(hexDump);
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Ошибка: {message}");
            Console.ResetColor();
        }
    }
}
