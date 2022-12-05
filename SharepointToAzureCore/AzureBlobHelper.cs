using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Options;
using SharepointToAzureCore.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzureCore
{
    public class AzureBlobHelper
    {
        private readonly IOptions<BptConfiguration> _bptConfiguration;
        public AzureBlobHelper(IOptions<BptConfiguration> bptConfiguration)
        {
            _bptConfiguration = bptConfiguration;
        }
        private  BlockBlobClient GetBlockBlobClient(string locationUrl, string fileUrl, bool createIfNotExists = true)
        {
            var containerName = locationUrl.GetContainerName();
            //var connectionString = "DefaultEndpointsProtocol=https;AccountName=bptagent;AccountKey=zai9Sqbw/DsvPLVyuU35sBrCCahmimb9wsTlayVIjlKRk8wgGC8hj10VQTvWt09WKXav1YKzESmkc5K3mVlGHw==;EndpointSuffix=core.windows.net";
            BlobContainerClient containerClient = new BlobContainerClient(_bptConfiguration.Value.AzureBlobConnectionString, containerName.ToLower());

            try
            {
                if (createIfNotExists)
                    containerClient.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                
            }

            BlockBlobClient blobClient = containerClient.GetBlockBlobClient(fileUrl);

            return blobClient;
        }

        public void UploadToAzureStorageBlob(string fileName, Stream stream, string locationUrl)
        {
            byte[] result = null;
            try
            {
                using (var streamReader = new MemoryStream())
                {
                    stream.CopyTo(streamReader);
                    result = streamReader.ToArray();
                }
                UploadToBlob(result, locationUrl, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
            }
        }

        private async Task UploadToBlob(byte[] content, string locationUrl, string fileName)
        {
            const int blockSize = 1 * 1024 * 1024; //1 MB Block
            var counter = 0;
            var blockIds = new List<string>();
            BlockBlobClient blobClient = GetBlockBlobClient(locationUrl, fileName);
            var fileExtension = Path.GetExtension(fileName)?.Replace('.', ' ').Trim();
            BlobHttpHeaders blobHeader = new BlobHttpHeaders()
            {
                ContentType = $"application/{fileExtension}"
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
