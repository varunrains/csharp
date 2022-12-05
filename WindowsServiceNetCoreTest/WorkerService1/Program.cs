using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //EventLog eventLog = new EventLog();
            //eventLog.Source = "MyEventLogTarget";
            //eventLog.WriteEntry($"Number of arguments passed {args?.Length}", System.Diagnostics.EventLogEntryType.Information);
            //if (args.Length > 0)
            //eventLog.WriteEntry($"First argument  :{args[0]} , Second argument : {args[1]}", System.Diagnostics.EventLogEntryType.Information);

            //var argu = Environment.GetCommandLineArgs();
            //eventLog.WriteEntry($"Number of arguments passed with environment variable {argu?.Length}", System.Diagnostics.EventLogEntryType.Information);
            //if (argu.Length == 1)
            //    eventLog.WriteEntry($"First argument  :{argu[0]}", System.Diagnostics.EventLogEntryType.Information);
            //if(argu.Length == 2)
            //    eventLog.WriteEntry($"Second argument :{argu[1]}", System.Diagnostics.EventLogEntryType.Information);
            //DirectoryInfo networkDir = new DirectoryInfo(Process.GetCurrentProcess().MainModule?.FileName);
            //eventLog.WriteEntry($"Running the windows service @: {Process.GetCurrentProcess().MainModule?.FileName}");
            //eventLog.WriteEntry($"Parent directory: { networkDir.Parent}");
            //eventLog.WriteEntry($"Parent parent directory: { networkDir.Parent.Parent}");
            //CreateHostBuilder(args).Build().Run();

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1641644372).AddMinutes(330);
            Console.WriteLine(dateTimeOffset);
            Console.WriteLine(DateTimeOffset.UtcNow.AddMinutes(-5).ToUnixTimeSeconds());
            Console.ReadLine();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                }).UseWindowsService();
    }
}
