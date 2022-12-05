using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzure
{
    public interface IFileProvider
    {
        Task<Stream> GetStream(string fileUrl, ClientContext ctx);

        ClientContext LoadClientContext(string baseSiteUrl);
    }
}
