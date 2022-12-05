using Microsoft.Extensions.Options;
using Microsoft.SharePoint.Client;
using SharepointToAzureCore.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzureCore
{
    public class SharePointOnlineHelper : IFileProvider
    {
        public ClientContext ClientContext;
        private readonly IOptions<BptConfiguration> _bptConfiguration;

        private const string SharePointOnlineCachePrefix = "SharepointOnline";
        private const int CacheDuration = 60 * 24 * 30;
        public SharePointOnlineHelper(IOptions<BptConfiguration> bptConfiguration)
        {
            //var locationUrl = "https://eurofinsbpt.sharepoint.com/sites/staging_general";
            //ClientContext = LoadClientContext(locationUrl);
            _bptConfiguration = bptConfiguration;
        }

        public ClientContext LoadClientContext(string baseSiteUrl)
        {
           // var clientContextFromCache = _memoryCacheContext.Get<ClientContext>($"{SharePointOnlineCachePrefix}_{baseSiteUrl}");
            
            //if (clientContextFromCache != null)
            //    return clientContextFromCache;

            var o365Password = new SecureString();

            foreach (char c in _bptConfiguration.Value.SharepointOnlinePassword)
            {
                o365Password.AppendChar(c);
            }
            var o365ClientContext = new ClientContext(baseSiteUrl);
            var o365Credentials = new SharePointOnlineCredentials(_bptConfiguration.Value.SharepointOnlineUserName, o365Password);
            o365ClientContext.Credentials = o365Credentials;

          //  _memoryCacheContext.Put(o365ClientContext, CacheDuration, SharePointOnlineCachePrefix);
            return o365ClientContext;
        }

        public Stream GetStream(string fileUrl)
        {
            var fileItem = ClientContext.Web.GetFileByServerRelativeUrl(fileUrl);
            var fileInfo = fileItem.OpenBinaryStream();
            ClientContext.ExecuteQueryRetry();
            return fileInfo.Value;
        }
    }
}
