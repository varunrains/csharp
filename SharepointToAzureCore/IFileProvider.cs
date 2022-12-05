using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzureCore
{
    public interface IFileProvider
    {
        Stream GetStream(string fileUrl);

        ClientContext LoadClientContext(string baseSiteUrl);
    }
}
