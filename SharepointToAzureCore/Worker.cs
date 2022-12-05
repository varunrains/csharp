using Microsoft.Extensions.Hosting;
using log4net;
using Microsoft.Extensions.Options;
using SharepointToAzureCore.Configuration;
using SharepointToAzureCore.DB;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net.Core;

namespace SharepointToAzureCore
{
    public class Worker: BackgroundService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Worker));

        private readonly IOptions<BptConfiguration> _bptConfiguration;
        public readonly SodiumRepository _sodiumRepository;
        public readonly SharePointOnlineHelper _sharePointOnlineHelper;
        public readonly SharePointOnPremHelper _sharePointOnPremHelper;
        public readonly AzureBlobHelper _azureBlobHelper;
        public readonly ConcurrentDictionary<Guid, string> FileStatus = new ConcurrentDictionary<Guid, string>();
        
        public Worker(
            SodiumRepository sodiumRepository,
            SharePointOnlineHelper sharePointOnlineHelper,
            SharePointOnPremHelper sharePointOnPremHelper,
            AzureBlobHelper azureBlobHelper,
            IOptions<BptConfiguration> bptConfiguration)
        {
            _sodiumRepository = sodiumRepository;
            _sharePointOnlineHelper = sharePointOnlineHelper;
            _sharePointOnPremHelper = sharePointOnPremHelper;
            _azureBlobHelper = azureBlobHelper;
            _bptConfiguration = bptConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
            
           var filesToMigrate =  await _sodiumRepository.GetFilesWithRepositoryConfiguration().ConfigureAwait(false);
            var parallelOption = new ParallelOptions() { MaxDegreeOfParallelism = _bptConfiguration.Value.BatchSize };
            var stopWatch = Stopwatch.StartNew();
           Parallel.ForEach(filesToMigrate, parallelOption, file => {
               IFileProvider provider = file.RepositoryType.Equals("Sharepoint", StringComparison.CurrentCultureIgnoreCase)  ? (IFileProvider) _sharePointOnPremHelper : _sharePointOnlineHelper;
               try
               {
                   provider.LoadClientContext(file.LocationUrl);
                   var stream = provider.GetStream(file.Url);

                   var fileName = file.Url.Split('/')[file.Url.Split('/').Length - 1];
                   // var containerToCopy = 
                   _azureBlobHelper.UploadToAzureStorageBlob(fileName, stream, $"https://{_bptConfiguration.Value.AzureBlobConnectionString.GetAccountName()}.blob.core.windows.net/migrate");
               }
               catch(Exception e)
               {
                   log.Error("exception occured while file transfer", e);
               }
           });
            log.Info($"Time taken for { _bptConfiguration.Value.BatchSize } file transfer");
        }
    }
}
