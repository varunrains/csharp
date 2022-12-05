using Eurofins.BPT.Caching;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzure
{
    public class SharePointOnlineHelper : IFileProvider
    {
        public ClientContext ClientContext;

        private const string SharePointOnlineCachePrefix = "SharepointOnline";
        private const int CacheDuration = 60 * 24 * 30;
        private readonly MemoryCacheContext _memoryCacheContext;
        public SharePointOnlineHelper(MemoryCacheContext memoryCacheContext)
        {
            //var locationUrl = "https://eurofinsbpt.sharepoint.com/sites/staging_general";
            //ClientContext = LoadClientContext(locationUrl);
            _memoryCacheContext = memoryCacheContext;
        }

        public ClientContext LoadClientContext(string baseSiteUrl)
        {
            var clientContextFromCache = _memoryCacheContext.Get<ClientContext>($"{SharePointOnlineCachePrefix}_{baseSiteUrl}");

            if (clientContextFromCache != null)
            {
                return clientContextFromCache;
            }

            var o365Password = new SecureString();

            var sharepointOnlineUsername = ConfigurationManager.AppSettings["SharepointOnlineUserName"];
            var sharepointOnlinePassword = ConfigurationManager.AppSettings["SharepointOnlinePassword"];

            foreach (char c in sharepointOnlinePassword)
            {
                o365Password.AppendChar(c);
            }
            var o365ClientContext = new ClientContext(baseSiteUrl);
            var o365Credentials = new SharePointOnlineCredentials(sharepointOnlineUsername, o365Password);
            o365ClientContext.Credentials = o365Credentials;

            _memoryCacheContext.Put(o365ClientContext, CacheDuration, $"{SharePointOnlineCachePrefix}_{baseSiteUrl}");
            return o365ClientContext;
        }

        public async Task<Stream> GetStream(string fileUrl, ClientContext ctx)
        {
            var fileItem = ctx.Web.GetFileByServerRelativeUrl(fileUrl);
            var fileInfo = fileItem.OpenBinaryStream();
           await ctx.ExecuteQueryRetryAsync().ConfigureAwait(false);
            return fileInfo.Value;
        }
    }
}
