﻿using Microsoft.Azure.KeyVault;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AuthenticationUsingKeyVaultCertificate
{
    class Program
    {
        public static void Main(string[] args)
        {

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //   var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
            //     async (string authority, string resource, string scope) =>
            //    {
            //        var authContext = new AuthenticationContext(authority);
            //        var credential = new ClientCredential("clientId", "clientSecret");
            //        AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential).ConfigureAwait(false);
            //        if (result == null)
            //        {
            //            throw new InvalidOperationException("Failed to get JWT token");
            //        }
            //        return result.AccessToken;
            //    }
            //));
            try
            {
                var authority = "";
                var resource = "";
                //Azure dev
                //var clientIdUri = "";
                //QA 172
                var clientIdUri = "";
                var authContext = new AuthenticationContext(authority);

                byte[] data = Convert.FromBase64String();
                string secret = Encoding.UTF8.GetString(data);

                //g5B72
                var credential = new ClientCredential(, secret);

                //QA 172
               // var credential = new ClientCredential("30dc1e16-39ae-49ca-9211-7423eaf8727f", "W1KbDWyXI7zI3AFMqFvy4ktwI91BzcUPGDIzY5VG5OY=");
                AuthenticationResult result = authContext.AcquireTokenAsync(clientIdUri, credential).ConfigureAwait(false).GetAwaiter().GetResult();
                var to = result.AccessToken;
                var certificate = GetCertificateFromKeyVaultSecret();
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
                //X509Certificate2Collection certCollection = null;
                //if (certCollection == null)
                //{

                //    certCollection = GetTrustedCertificatesFromStore();
                //}
                //var certificateCollection = certCollection.Find(X509FindType.FindBySubjectKeyIdentifier, "9ca145e1979c0125fc49504cf45c2a27e1154207", false);

                //if (certificateCollection != null && certificateCollection.Count > 0)
                //{
                //    handler.ClientCertificates.Add(certificateCollection[0]);
                //}

                //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate1, chain, sslPolicyErrors) =>
                //{
                //    if (sslPolicyErrors == SslPolicyErrors.None)
                //    {
                //        return true;
                //    }

                  
                //        if (certificate.Thumbprint == certificate1.GetCertHashString())
                //            return true;
                  

                //    return false;
                //};

                if (certificate != null)
                    handler.ClientCertificates.Add(certificate);

                using (var _restClient = new HttpClient(handler))
                {
                    SetDestinationServiceToken(_restClient.DefaultRequestHeaders, to);
                    // _restClient.Timeout = TimeSpan.FromMinutes(_crossInstancePushConfiguration.ClientTimeoutMilliseconds);
                    var d = _restClient.GetByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    //var ds = _restClient.GetAsync("https://in01apvt172:65435/api/businessunits/6A22FA2C-E1B7-4FAE-89BF-04211C8E91F2").ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }catch(Exception e)
            {

            }
        }

        private static X509Certificate2Collection GetTrustedCertificatesFromStore()
        {
            using (X509Store certStore = new X509Store(StoreName.My, (StoreLocation)Enum.Parse(typeof(StoreLocation), "CurrentUser")))
            {
                certStore.Open(OpenFlags.ReadOnly);
                return certStore.Certificates;
            }
        }

        private static KeyVaultClient GetKeyVaultClient()
        {
            var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
              async (string authority, string resource, string scope) =>
              {
                  var authContext = new AuthenticationContext(authority);
                  var credential = new ClientCredential();
                  AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential).ConfigureAwait(false);
                  if (result == null)
                  {
                      throw new InvalidOperationException("Failed to get JWT token");
                  }
                  return result.AccessToken;
              }
         ));

            return kvc;
        }

        private static X509Certificate2 GetCertificateFromKeyVaultSecret()
        {
           var itaagCode = "ITAAG002";
           var serviceName = "Sodium";
            var certificateName = "PreAuthCertificate";
           // var cachedCertificate = _memoryCacheContext.Get<X509Certificate2>($"{CacheKeyPrefix}{itaagCode}-{serviceName}-{certificateName}");

            //if (cachedCertificate != null && cachedCertificate.HasPrivateKey)
            //    return cachedCertificate;

            try
            {
                var kvc = GetKeyVaultClient();
                var certificateSecret = kvc.GetSecretAsync("https://crossinstancesharing.vault.azure.net", $"{itaagCode}-{serviceName}-{certificateName}").ConfigureAwait(false).GetAwaiter().GetResult();
                var certificate = Convert.FromBase64String(certificateSecret.Value);
                var x509Certificate = new X509Certificate2(certificate);

                if (x509Certificate != null && x509Certificate.HasPrivateKey)
                return x509Certificate.;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void SetDestinationServiceToken(HttpRequestHeaders headers, string token)
        {
            //var token = GetAccessTokenForSourceItaag(itaagCode);
            headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
    }
}
