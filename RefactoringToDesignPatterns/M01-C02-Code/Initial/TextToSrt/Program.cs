using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TextToSrt
{
    class Program
    {
        private static string ToolName => Assembly.GetExecutingAssembly().GetName().Name;

        private static string UsageText =>
            $"{ToolName} <source file>.txt <output file>.srt";

        static void ShowUsage() =>
            Console.WriteLine(UsageText);
         
        static bool Verify(string[] args) =>
            args.Length == 2 &&
            File.Exists(args[0]);

        static void Process(FileInfo source, FileInfo destination)
        {
        }

        static void Main(string[] args)
        {
            if (Verify(args))
                Process(new FileInfo(args[0]), new FileInfo(args[1]));
            else
                ShowUsage();
        }
    }
}
