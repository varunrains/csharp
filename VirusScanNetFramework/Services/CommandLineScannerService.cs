using System;
using System.Diagnostics;
using System.IO;

namespace VirusScanNetFramework.Services
{
    public class CommandLineScannerService
    {
        private const string ScanFilePathPlaceHolder = "{scanFilePath}";
        private const string ReportFilePathPlaceHolder = "{reportFilePath}";




        public void ExecuteVirusScan(string path)
        {
            //var reportFilePath = $"{Path.Combine(@"C:\AntiVirusTest\ReportFolder", batchName)}";

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
                WindowStyle = ProcessWindowStyle.Maximized
            };
                
            try
            {
                var scanProcess = Process.Start(startInfo);
                scanProcess.WaitForExit(180* 1000);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing the virus scan for batch {batchName}", ex);
            }

           // return ProcessReportFile(reportFilePath);
        }

        //private VirusScanReport.Scan ProcessReportFile(string reportFilePath)
        //{
        //    if (!File.Exists(reportFilePath))
        //    {
        //        throw new Exception("There was a problem executing the virus scan: no report file found");
        //    }

        //    try
        //    {
        //        var serializer = new XmlSerializer(typeof(VirusScanReport.Scan));
        //        using var fileReader = new FileStream(reportFilePath, FileMode.Open);
        //        using var xmlReader = XmlReader.Create(fileReader);
        //        if (!(serializer.Deserialize(xmlReader) is VirusScanReport.Scan result))
        //        {
        //            throw new Exception("There was a problem executing the virus scan: invalid report file");
        //        }
        //        return result;
        //    }
        //    finally
        //    {
        //        _fileSystemService.TryDeleteFile(reportFilePath, 2);
        //    }
        //}
    }
}
