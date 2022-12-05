using System;
using System.Collections.Generic;
using System.Text;

namespace SharepointToAzureCore.Configuration
{
    public class BptConfiguration
    {
        public string AzureBlobConnectionString { get; set; }
        public int BlockSizeForBlockBlob { get; set; }
        public string SharepointOnlineUserName { get; set; }
        public string SharepointOnlinePassword { get; set; }
        public string SharepointOnPremUserName { get; set; }
        public string SharepointOnPremPassword { get; set; }
        public string SharepointOnPremDomain { get; set; }
        public string DBConnectionString { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StripSharepointOnPremLocationUrl { get; set; }
        public string StripSharepointOnlineLocationUrl { get; set; }
        public int BatchSize { get; set; }

    }
}
