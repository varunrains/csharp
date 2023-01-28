
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzure
{
    public class SharePointOnPremHelper : IFileProvider
    {
        private const int RequestTimeout = 60 * 60 * 1000;
        private readonly MemoryCacheContext _memoryCacheContext;

        private const string SharePointOnPremCachePrefix = "SharepointOnPrem";
        private const int CacheDuration = 100;
        public SharePointOnPremHelper(MemoryCacheContext memoryCacheContext)
        {
           
            //ClientContext = LoadClientContext(baseSiteUrl);
            _memoryCacheContext = memoryCacheContext;
        }

        public ClientContext LoadClientContext(string baseSiteUrl)
        {

            var clientContextFromCache = _memoryCacheContext.Get<ClientContext>($"{SharePointOnPremCachePrefix}_{baseSiteUrl}");
            if (clientContextFromCache != null)
                return clientContextFromCache;

            var cc = new ClientContext(baseSiteUrl)
            {
                Credentials = LoadSharePointClientCredentials(),
                RequestTimeout = RequestTimeout
            };

            _memoryCacheContext.Put(cc, CacheDuration, $"{SharePointOnPremCachePrefix}_{baseSiteUrl}");
            return cc;
        }

        public async Task<Stream> GetStream(string fileUrl, ClientContext ctx)
        {
            await ctx.ExecuteQueryRetryAsync().ConfigureAwait(false);
            var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(ctx, fileUrl);
            return fileInfo.Stream;
        }

        internal NetworkCredential LoadSharePointClientCredentials()
        {
            var sharepointOnpremUserName = ConfigurationManager.AppSettings["SharepointOnPremUserName"];
            var sharepointOnpremPassword = ConfigurationManager.AppSettings["SharepointOnPremPassword"];
            var domain = ConfigurationManager.AppSettings["SharepointOnPremDomain"];

            return new NetworkCredential(
                sharepointOnpremUserName,
                 sharepointOnpremPassword,
                domain);
        }
    }
}
