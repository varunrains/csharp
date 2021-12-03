using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerUploadAzure
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=bptagenttest;AccountKey=G75szURVcqAnUE/CazKYOfuFsgY9NdNnlcPhqp1pCFH8Uk38xj7xqv9FCq8SY6ImUS6ygfUAsjQSGMwqwNCoig==;EndpointSuffix=core.windows.net";
            var blobServiceClient = new BlobServiceClient(connectionString);

            //await Upload(blobServiceClient);
            await DownloadAllItemsFromContainer(blobServiceClient);
        }

        private static async Task Upload(BlobServiceClient blobServiceClient)
        {
            var guid = Guid.NewGuid();
            try
            {
                blobServiceClient.CreateBlobContainer($"{Guid.Parse("ec0ca571-64aa-482a-9350-039068247fac")}");
            }
            catch (RequestFailedException ex) when (ex.Message.Contains("container already exists"))
            {

            }
            
            var containerClient = blobServiceClient.GetBlobContainerClient(guid.ToString());
            //var d = containerClient.GetBlobs();

            var blobClient = containerClient.GetBlobClient(guid.ToString());
            var message = "Hi how are you?>";
            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(message))).ConfigureAwait(false);
        }

        private static async Task DownloadAllItemsFromContainer(BlobServiceClient blobServiceClient)
        {
            try
            {
                //blobServiceClient.CreateBlobContainer($"{Guid.Parse("ec0ca571-64aa-482a-9350-039068247fac")}");
             var containerClient =   blobServiceClient.GetBlobContainerClient($"{Guid.Parse("ec0ca571-64aa-482a-9350-039068247fac")}");
              ///  var blobClient = containerClient.GetBlobClient(claimCheck.ClaimCheck.ToString());
              //containerClient.GetBlobs
               var d = containerClient.GetBlobs();
                var ds = d.Select(x => x);
                foreach (var das in ds)
                {
                    var blobClient = containerClient.GetBlobClient(das.Name);
                    BlobDownloadInfo download = await blobClient.DownloadAsync();
                    //await download.Content.CopyTo()
                }
            }
            catch (RequestFailedException ex) when (ex.Message.Contains("container already exists"))
            {

            }
        }
    }
}
