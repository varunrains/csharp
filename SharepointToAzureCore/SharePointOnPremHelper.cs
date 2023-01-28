using Microsoft.Extensions.Options;
using Microsoft.SharePoint.Client;
using SharepointToAzureCore.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzureCore
{
    public class SharePointOnPremHelper : IFileProvider
    {
        private const int RequestTimeout = 60 * 60 * 1000;
        private static ClientContext ClientContext;
        private readonly IOptions<BptConfiguration> _bptConfiguration;

        private const string SharePointOnPremCachePrefix = "SharepointOnPrem";
        private const int CacheDuration = 100;
        public SharePointOnPremHelper(IOptions<BptConfiguration> bptConfiguration)
        {
            
            //ClientContext = LoadClientContext(baseSiteUrl);
            _bptConfiguration = bptConfiguration;
            //_memoryCacheContext = memoryCacheContext;
        }

        public  ClientContext LoadClientContext(string baseSiteUrl)
        {
            var clientContextFromCache = MemoryCacheContext.GetValue<ClientContext>($"{SharePointOnPremCachePrefix}_{baseSiteUrl}");

            if (clientContextFromCache != null)
                return clientContextFromCache;

            var cc = new ClientContext(baseSiteUrl)
            {
                Credentials = LoadSharePointClientCredentials(),
                RequestTimeout = RequestTimeout
            };

            MemoryCacheContext.PutValue<ClientContext>(baseSiteUrl, cc);
            return cc;
        }

        public  Stream GetStream(string fileUrl)
        {
            ClientContext.ExecuteQueryRetry();
            var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(ClientContext, fileUrl);
            return fileInfo.Stream;
        }

        internal NetworkCredential LoadSharePointClientCredentials()
        {
            return new NetworkCredential(
                _bptConfiguration.Value.SharepointOnPremUserName,
                 _bptConfiguration.Value.SharepointOnPremPassword,
                 _bptConfiguration.Value.SharepointOnPremDomain);
        }
    }
}
