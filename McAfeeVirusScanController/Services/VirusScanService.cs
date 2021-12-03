using McAfeeVirusScanController.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirusScanApi.Services;

namespace McAfeeVirusScanController.Services
{
    public class VirusScanService
    {
        private readonly VirusScanConfiguration _config;
        private readonly FileSystemService _fileSystemService;
        private readonly CommandLineScannerService _scannerService;

        public VirusScanService(
            IOptions<VirusScanConfiguration> options,
            FileSystemService fileSystemService,
            CommandLineScannerService scannerService)
        {
            _config = options?.Value;
            _fileSystemService = fileSystemService;
            _scannerService = scannerService;
        }

        public async Task ScanFilesAsync(IFormFileCollection files)
        {
            // Make sure we have some files that aren't null
            if (files == null || !files.Any(file => file != null))
            {
                return;
            }

            var batchName = Guid.NewGuid().ToString();
            var batchPath = Path.Combine(_config.UploadFolderPath, batchName);
            try
            {
                // We'll save everything to a new unique batch folder so we don't run into problems with existing files
                // and it allows us to clean up everything in one shot by just deleting the folder
                _fileSystemService.CreateDirectory(batchPath);

                // Make sure we have the report folder (will only create it if it doesn't exist)
                _fileSystemService.CreateDirectory(_config.ReportFolderPath);

                var fileInfos = new Dictionary<string, string>();
                foreach (var file in files.Where(file => file != null))
                {
                    // Each file just gets a unique name so we aren't creating files with potentially unsafe file names
                    var filePath = Path.Combine(batchPath, Guid.NewGuid().ToString());
                    fileInfos.Add(file.FileName, filePath);
                    await _fileSystemService.SaveFile(file, filePath).ConfigureAwait(false);
                }
               _scannerService.ExecuteVirusScan(batchName, fileInfos.Values.ToList());

                //var results = BuildVirusScanResults(fileInfos, report);
                //return results;
            }
            finally
            {
                _fileSystemService.DeleteDirectory(batchPath);
            }
        }

        //private IEnumerable<VirusScanResultDto> BuildVirusScanResults(Dictionary<string, string> fileInfos, VirusScanReport.Scan report)
        //{
        //    var results = fileInfos.Select(fi => BuildVirusScanResult(fi.Key, fi.Value, report));
        //    return results;
        //}

        //private VirusScanResultDto BuildVirusScanResult(string resultId, string filePath, VirusScanReport.Scan report)
        //{
        //    var summary = report.Summary?.FirstOrDefault(s => s.OnPath.Equals(filePath, StringComparison.OrdinalIgnoreCase));
        //    if (summary == null)
        //    {
        //        // If the summary is missing, it may mean that another virus scan process deleted the file
        //        // so we'll treat it as a potential threat
        //        summary = new VirusScanReport.Summary
        //        {
        //            Totalfiles = 1,
        //            PossiblyInfected = 1
        //        };

        //        // If the file was missing, dummy up file info to indicate a potentially infected file
        //        var missingFile = new VirusScanReport.File
        //        {
        //            name = filePath,
        //            status = "infected",
        //            detectiontype = "missing-file",
        //            virusname = "unknown"
        //        };
        //        // Add the new dummy file info to the array (building the array if none exists)
        //        report.File = (report.File ?? Array.Empty<VirusScanReport.File>()).Append(missingFile).ToArray();
        //    }

        //    var fileFolder = Path.GetDirectoryName(filePath);

        //    return new VirusScanResultDto
        //    {
        //        ResultId = resultId,
        //        IsClean = summary.Totalfiles > 0 && summary.PossiblyInfected == 0,
        //        Report = new VirusScanReportDto
        //        {
        //            ProductName = report.Preamble.Product_name?.value,
        //            ProductVersion = report.Preamble.Version?.value,
        //            AvEngineVersion = report.Preamble.AV_Engine_version?.value.ToString(),
        //            DatVersion = report.Preamble.Dat_set_version?.value.ToString(),
        //            Options = report.Options?.value?
        //                // Hide folder paths in output
        //                .Replace(_config.ReportFolderPath, "", StringComparison.OrdinalIgnoreCase)
        //                .Replace(_config.UploadFolderPath, "", StringComparison.OrdinalIgnoreCase),
        //            DateTime = report.Date_Time?.value,
        //            Duration = report.Time?.value,
        //            TotalFilesClean = summary.Clean,
        //            TotalFilesScanned = summary.Totalfiles,
        //            TotalFilesNotScanned = summary.NotScanned,
        //            TotalFilesPossiblyInfected = summary.PossiblyInfected,
        //            Files = report.File?.Where(f => f.name.StartsWith(filePath, StringComparison.OrdinalIgnoreCase))
        //                .Select(f => new VirusScanFileInfo
        //                {
        //                    Name = f.name.Replace(fileFolder, "", StringComparison.OrdinalIgnoreCase),
        //                    Status = f.status,
        //                    DetectionType = f.detectiontype,
        //                    VirusName = f.virusname
        //                })
        //        }
        //    };
        //}
    }
}
