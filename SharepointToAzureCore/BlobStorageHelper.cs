using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzureCore
{
    public static class BlobStorageHelper
    {
        public static string GetAccountKey(this string connectionString)
        {
            var AccountKeyWithLabel = connectionString.Split(';')[2];
            return AccountKeyWithLabel.Substring(AccountKeyWithLabel.IndexOf("=")+1);
        }

        public static string GetAccountName(this string connectionString)
        {
            return connectionString.Split(';')[1].Split('=')[1];
        }

        public static string GetDefaultEndPointProtocol(this string connectionString)
        {
            return connectionString.Split(';')[0].Split('=')[1];
        }

        public static string GetContainerName(this string locationUrl)
        {
            return locationUrl.Split('/').GetValue(locationUrl.Split('/').Length - 1).ToString();
        }
    }
}
