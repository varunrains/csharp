using Azure.Storage.Blobs;
using log4net;
using SharepointToAzure.DB;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharepointToAzure
{
    public class Worker
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Worker));

        public readonly SodiumRepository _sodiumRepository;
        public readonly SharePointOnlineHelper _sharePointOnlineHelper;
        public readonly SharePointOnPremHelper _sharePointOnPremHelper;
        public readonly AzureBlobHelper _azureBlobHelper;
        public readonly ConcurrentDictionary<Guid, long> FileWithSize = new ConcurrentDictionary<Guid, long>();
        public Worker(
            SodiumRepository sodiumRepository,
            SharePointOnlineHelper sharePointOnlineHelper,
            SharePointOnPremHelper sharePointOnPremHelper,
            AzureBlobHelper azureBlobHelper
            )
        {
            _sodiumRepository = sodiumRepository;
            _sharePointOnlineHelper = sharePointOnlineHelper;
            _sharePointOnPremHelper = sharePointOnPremHelper;
            _azureBlobHelper = azureBlobHelper;
        }

        public  async Task ExecuteAsync()
        {
            var batchSize = ConfigurationManager.AppSettings["BatchSize"];
            var azureBlobConnectionString = ConfigurationManager.AppSettings["AzureBlobConnectionString"];
          //  var endDate = ConfigurationManager.AppSettings["EndDate"];

            var filesToMigrate = await _sodiumRepository.GetFilesWithRepositoryConfiguration().ConfigureAwait(false);
            var parallelOption = new ParallelOptions() { MaxDegreeOfParallelism = int.Parse(batchSize) };
            log.Info($"Started file transfer, received {filesToMigrate.Count} file records from DB");
            var stopWatch = Stopwatch.StartNew();

            Parallel.ForEach(filesToMigrate, parallelOption, (file) => {
                IFileProvider provider = file.RepositoryType.Equals("Sharepoint", StringComparison.CurrentCultureIgnoreCase) ? (IFileProvider)_sharePointOnPremHelper : _sharePointOnlineHelper;
                try
                {
                    var ctx = provider.LoadClientContext(file.LocationUrl);
                    var stream = provider.GetStream(file.Url, ctx).ConfigureAwait(false).GetAwaiter().GetResult();
                    //var bytes = ConvertToBytes(stream, file.FileId).ConfigureAwait(false).GetAwaiter().GetResult();
                   // containerClient.UploadBlobAsync("sd", stream);
                   // var d = stream.Length;
                    var fileName = file.Url.Split('/')[file.Url.Split('/').Length - 1];
                    // var containerToCopy = 
                    // _azureBlobHelper.UploadToBlob(bytes, $"https://{azureBlobConnectionString.GetAccountName()}.blob.core.windows.net/migrate", fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    _azureBlobHelper.UploadToBlob(stream, $"https://{azureBlobConnectionString.GetAccountName()}.blob.core.windows.net/migrate", fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                    //var size = bytes.Length / 1000;
                    FileWithSize.TryAdd(file.FileId, 0);
                }
                catch (Exception e)
                {
                    log.Error("exception occured while file transfer", e);
                }
            });
            var totalFileSize = FileWithSize.Sum(x => x.Value);
            log.Info($"Time taken to transfer {FileWithSize.Count} files of size { totalFileSize } Kbyte  is {stopWatch.ElapsedMilliseconds / 1000} seconds. Batch size used {batchSize} parallel processing");
        }

        public async Task<byte[]> ConvertToBytes(Stream stream, Guid fileId)
        {
            byte[] result = null;
            try
            {
                using (var streamReader = new MemoryStream())
                {
                    await stream.CopyToAsync(streamReader).ConfigureAwait(false);
                    result = streamReader.ToArray();
                }
                return result;
            }
            catch (Exception ex)
            {
                log.Info($"Error occured while converting stream to byte array for file with fileId {fileId.ToString()}", ex);
                return null;
            }
        }
    }
}
