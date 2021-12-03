using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace McAfeeCommandLineAntiVirusScan
{
    class Program
    {
        public static ConcurrentDictionary<int, string> pathToScan = new ConcurrentDictionary<int, string>();
        //public static Stopwatch timer = new Stopwatch();
        static void Main(string[] args)
        {
            var numberOfParallelInstance = 1;
            //timer.Start();
            pathToScan.TryAdd(1, @"C:\AntiVirusTest\5mb");
            pathToScan.TryAdd(2, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.TryAdd(3, @"C:\AntiVirusTest\Files");
            pathToScan.TryAdd(4, @"C:\AntiVirusTest\filesToScan");

            pathToScan.TryAdd(5, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.TryAdd(6, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.TryAdd(7, @"C:\AntiVirusTest\Files");
            pathToScan.TryAdd(8, @"C:\AntiVirusTest\filesToScan");
            pathToScan.TryAdd(9, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.TryAdd(10, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.TryAdd(11, @"C:\AntiVirusTest\Files");
            pathToScan.TryAdd(12, @"C:\AntiVirusTest\filesToScan");
            pathToScan.TryAdd(13, @"C:\AntiVirusTest\5mb");
            pathToScan.TryAdd(14, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.TryAdd(15, @"C:\AntiVirusTest\Files");
            pathToScan.TryAdd(16, @"C:\AntiVirusTest\filesToScan");

            pathToScan.TryAdd(17, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.TryAdd(18, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.TryAdd(19, @"C:\AntiVirusTest\5mb");
            pathToScan.TryAdd(20, @"C:\AntiVirusTest\filesToScan");
            pathToScan.TryAdd(21, @"C:\AntiVirusTest\40mb");
            pathToScan.TryAdd(22, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.TryAdd(23, @"C:\AntiVirusTest\Files");
            pathToScan.TryAdd(24, @"C:\AntiVirusTest\filesToScan");

            Console.WriteLine($"Number of parallel scans :: {numberOfParallelInstance}");
            Parallel.ForEach(Enumerable.Range(1, numberOfParallelInstance), (i) =>
            {
                var path = pathToScan.First(x => x.Key == i).Value;

                ExecuteVirusScan(path);
            });


        }

        public static void ExecuteVirusScan(string path)
        {
            var timer = Stopwatch.StartNew();
            var batchName = Guid.NewGuid().ToString();
            var reportFilePath = $"{Path.Combine(@"C:\AntiVirusTest\ReportFolder", batchName)}";
           // var scanPathFolder = $"{Path.Combine(@"C:\AntiVirusTest\ScanFolder1")}";
            var scanPathFolder = $"{Path.Combine(path)}";

            // Pass in a space-separated list of the uploaded files to be scanned which replaces placeholder
            // and replace placeholder with actual reportFilePath
            //var args = _config.ScannerArguments
            //    .Replace(ScanFilePathPlaceHolder, string.Join(' ', filePaths.Select(f => $"\"{f}\"")), StringComparison.OrdinalIgnoreCase)
            //    .Replace(ReportFilePathPlaceHolder, $"\"{reportFilePath}\"", StringComparison.OrdinalIgnoreCase);
            var args = $"/silent /secure /rptall /rpterr /timeout=90 /xmlpath={reportFilePath} {scanPathFolder}";
            var startInfo = new ProcessStartInfo
            {
                FileName = $@"C:\McAfeeAntivirus\scan.exe",
                Arguments = args,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Minimized
            };

            try
            {
               
                var scanProcess = Process.Start(startInfo);
                scanProcess?.WaitForExit(180* 1000);
                var dirInfo = new DirectoryInfo(scanPathFolder);
                FileInfo[] files = dirInfo.GetFiles();
                long dirSize = 0;
                foreach (FileInfo file in files)
                {
                    dirSize += file.Length;
                }

                Console.WriteLine($"Time taken: {timer.Elapsed.Minutes} minute {timer.Elapsed.Seconds} seconds for size :{(dirSize/1000)/1000} MB");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing the virus scan for batch {batchName}", ex);
            }

            //return ProcessReportFile(reportFilePath);
        }
    }
}
