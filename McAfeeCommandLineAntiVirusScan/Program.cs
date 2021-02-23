using System;
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
        public static Dictionary<int, string> pathToScan = new Dictionary<int, string>();
        public static Stopwatch timer = new Stopwatch();
        static void Main(string[] args)
        {
            var numberOfParallelInstance = 12;
            timer.Start();
            pathToScan.Add(1, @"C:\AntiVirusTest\5mb");
            pathToScan.Add(2, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.Add(3, @"C:\AntiVirusTest\Files");
            pathToScan.Add(4, @"C:\AntiVirusTest\filesToScan");

            pathToScan.Add(5, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.Add(6, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.Add(7, @"C:\AntiVirusTest\Files");
            pathToScan.Add(8, @"C:\AntiVirusTest\filesToScan");
            pathToScan.Add(9, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.Add(10, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.Add(11, @"C:\AntiVirusTest\Files");
            pathToScan.Add(12, @"C:\AntiVirusTest\filesToScan");
            pathToScan.Add(13, @"C:\AntiVirusTest\5mb");
            pathToScan.Add(14, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.Add(15, @"C:\AntiVirusTest\Files");
            pathToScan.Add(16, @"C:\AntiVirusTest\filesToScan");

            pathToScan.Add(17, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.Add(18, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.Add(19, @"C:\AntiVirusTest\Files");
            pathToScan.Add(20, @"C:\AntiVirusTest\filesToScan");
            pathToScan.Add(21, @"C:\AntiVirusTest\ScanFolder1");
            pathToScan.Add(22, @"C:\AntiVirusTest\ScanFolder");
            pathToScan.Add(23, @"C:\AntiVirusTest\Files");
            pathToScan.Add(24, @"C:\AntiVirusTest\filesToScan");

            Console.WriteLine($"Number of parallel scans :: {numberOfParallelInstance}");
            Parallel.ForEach(Enumerable.Range(1, numberOfParallelInstance), (i) =>
            {
                var path = pathToScan.First(x => x.Key == i).Value;

                ExecuteVirusScan(path);
            });


        }

        public static void ExecuteVirusScan(string pathToScan)
        {
            var batchName = Guid.NewGuid().ToString();
            var reportFilePath = $"{Path.Combine(@"C:\AntiVirusTest\ReportFolder", batchName)}";
           // var scanPathFolder = $"{Path.Combine(@"C:\AntiVirusTest\ScanFolder1")}";
            var scanPathFolder = $"{Path.Combine(pathToScan)}";

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

                Console.WriteLine($"Time taken: {timer.Elapsed.Seconds} seconds for size :{(dirSize/1000)/1000} MB");
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
