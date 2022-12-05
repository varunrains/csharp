using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=bptagent;AccountKey=zai9Sqbw/DsvPLVyuU35sBrCCahmimb9wsTlayVIjlKRk8wgGC8hj10VQTvWt09WKXav1YKzESmkc5K3mVlGHw==;EndpointSuffix=core.windows.net");
            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            var certCollection = GetTrustedCertificatesFromStore("CurrentUser");
            X509Certificate2 certificate = (X509Certificate2)certCollection.Find(X509FindType.FindBySubjectKeyIdentifier, "16ba277bfe347426bd05bc59e202720919717a56", false)[0];
            handler.ClientCertificates.Add(certificate);
            handler.UseDefaultCredentials = false;
            handler.PreAuthenticate = true;
            //stlimdevqueues.blob.dev.eurofinsbpt.com
            //handler.
            //DelegatingHandlerImpl delegatingHandlerImpl = new DelegatingHandlerImpl(handler);
           // CloudBlobClient 
             CloudQueueClient cloudQueueClient = new CloudQueueClient(storageAccount.QueueStorageUri, storageAccount.Credentials);
            var queue = cloudQueueClient.GetQueueReference("queue1");
            var ds = queue.GetMessage();
            var dss = new CloudQueueMessage("as");

            queue.AddMessage(dss);
            queue.DeleteMessage(dss);
            Console.WriteLine("Hello World!");
        }

        private static X509Certificate2Collection GetTrustedCertificatesFromStore(string location)
        {
            using (X509Store certStore = new X509Store(StoreName.My, (StoreLocation)Enum.Parse(typeof(StoreLocation), location)))
            {
                certStore.Open(OpenFlags.ReadOnly);
                return certStore.Certificates;
            }
        }
    }
}
