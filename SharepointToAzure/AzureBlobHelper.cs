using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Eurofins.BPT.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzure
{
    public class AzureBlobHelper
    {
        private readonly MemoryCacheContext _memoryCacheContext;
        private const string AzureContainerClientCachePrefix = "AzureContainerClientCachePrefix";
        private const int CacheDuration = 60 * 24 * 30;

        public AzureBlobHelper(MemoryCacheContext memoryCacheContext)
        {
            _memoryCacheContext = memoryCacheContext;
        }

        private BlobContainerClient GetBlobContainerClient(string locationUrl, bool createIfNotExists = true)
        {
            var containerName = locationUrl.GetContainerName();
            var azureBlobConnectionString = ConfigurationManager.AppSettings["AzureBlobConnectionString"];

            //var containerClientFromCache = _memoryCacheContext.Get<BlobContainerClient>($"{AzureContainerClientCachePrefix}_{containerName.ToLower()}");

            //if (containerClientFromCache != null)
            //{
            //    return containerClientFromCache;
            //}

            BlobContainerClient containerClient = new BlobContainerClient(azureBlobConnectionString, containerName.ToLower());

            try
            {
                if (createIfNotExists)
                    containerClient.CreateIfNotExists();
            }
            catch (Exception ex)
            {

            }
            //_memoryCacheContext.Put(containerClient, CacheDuration, $"{AzureContainerClientCachePrefix}_{containerName.ToLower()}");
            return containerClient;
        }

        private BlockBlobClient GetBlockBlobClient(string locationUrl, string fileUrl, bool createIfNotExists = true)
        {

            BlobContainerClient containerClient = GetBlobContainerClient(locationUrl);
            BlockBlobClient blobClient = containerClient.GetBlockBlobClient(fileUrl);

            return blobClient;
        }

        //public void UploadToAzureStorageBlob(string fileName, byte[] bytes, string locationUrl)
        //{
        //        UploadToBlob(bytes, locationUrl, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
        //}

        private string SetContentType(string fileExtension)
        {
            //https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
            switch (fileExtension)
                {
                case "ppt":
                    return "application/vnd.ms-powerpoint";
                case "pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "txt":
                    return "text/plain";
                case "ttf":
                    return "font/ttf";
                case "vsd":
                    return "application/vnd.visio";
                case "xls":
                    return "application/vnd.ms-excel";
                case "xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "gif":
                    return "image/gif";
                case "jpeg":
                    return "image/jpeg";
                case "jpg":
                    return "image/jpeg";
                case "pdf":
                    return "application/pdf";
                case "svg":
                    return "image/svg+xml";
                case "png":
                    return "image/png";
                case "7z":
                    return "application/x-7z-compressed";
                default:
                    return $"application/{fileExtension}";
            }
        }

        public async Task UploadToBlob(Stream stream, string locationUrl, string fileName)
        {
            var containerClient = GetBlobContainerClient(locationUrl);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            var fileExtension = Path.GetExtension(fileName)?.Replace('.', ' ').Trim();

            var contentType = SetContentType(fileExtension);

            BlobHttpHeaders blobHeader = new BlobHttpHeaders()
            {
                ContentType = contentType
            };
            BlobUploadOptions blobOptions = new BlobUploadOptions() { HttpHeaders = blobHeader };
            await blobClient.UploadAsync(stream,blobOptions).ConfigureAwait(false);
        }

        public async Task UploadToBlob(byte[] content, string locationUrl, string fileName)
        {
            const int blockSize = 1 * 1024 * 1024; //1 MB Block
            var counter = 0;
            var blockIds = new List<string>();
            BlockBlobClient blobClient = GetBlockBlobClient(locationUrl, fileName);
            var fileExtension = Path.GetExtension(fileName)?.Replace('.', ' ').Trim();

            var contentType = SetContentType(fileExtension);

            BlobHttpHeaders blobHeader = new BlobHttpHeaders()
            {
                ContentType = contentType
            };

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            using (var fileStream = new MemoryStream(content))
            {
                int dataRead;
                for (var bytesRemaining = content.Length; bytesRemaining > 0; bytesRemaining -= dataRead)
                {
                    var dataToRead = Math.Min(bytesRemaining, blockSize);
                    var data = new byte[dataToRead];
                    dataRead = await fileStream.ReadAsync(data, 0, (int)dataToRead).ConfigureAwait(false);
                    if (dataRead > 0)
                    {
                        var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(counter.ToString("d6")));
                        await blobClient.StageBlockAsync(blockId, new MemoryStream(data)).ConfigureAwait(false);
                        blockIds.Add(blockId);
                        counter++;
                    }
                }
                await blobClient.CommitBlockListAsync(blockIds, blobHeader).ConfigureAwait(false);
            }
            stopWatch.Stop();

        }
    }
}
