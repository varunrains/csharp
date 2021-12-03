using McAfeeVirusScanController.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VirusScanApi.Services
{
    public class CommandLineScannerService
    {
        private const string ScanFilePathPlaceHolder = "{scanFilePath}";
        private const string ReportFilePathPlaceHolder = "{reportFilePath}";

        private readonly VirusScanConfiguration _config;
        private readonly FileSystemService _fileSystemService;


        public CommandLineScannerService(IOptions<VirusScanConfiguration> options, FileSystemService fileSystemService)
        {
            _config = options?.Value;
            _fileSystemService = fileSystemService;
        }

        public void ExecuteVirusScan(string batchName, List<string> filePaths)
        {
            var reportFilePath = $"{Path.Combine(_config.ReportFolderPath, batchName)}";

            // Pass in a space-separated list of the uploaded files to be scanned which replaces placeholder
            // and replace placeholder with actual reportFilePath
            var args = _config.ScannerArguments
                .Replace(ScanFilePathPlaceHolder, string.Join(' ', filePaths.Select(f => $"\"{f}\"")), StringComparison.OrdinalIgnoreCase)
                .Replace(ReportFilePathPlaceHolder, $"\"{reportFilePath}\"", StringComparison.OrdinalIgnoreCase);

            var startInfo = new ProcessStartInfo
            {
                FileName = $"\"{_config.ScannerPath}\"",
                Arguments = args,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Maximized
            };

            try
            {
                var scanProcess = Process.Start(startInfo);
                scanProcess.WaitForExit(_config.ScannerProcessTimeoutSeconds * 1000);
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
