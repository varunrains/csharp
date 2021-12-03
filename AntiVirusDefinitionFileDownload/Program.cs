using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO.Compression;
using System.IO;

namespace AntiVirusDefinitionFileDownload
{
    //Important links to note
    //https://stackoverflow.com/questions/40187153/httpclient-in-using-statement
    //https://www.infoworld.com/article/3198673/when-to-use-webclient-vs-httpclient-vs-httpwebrequest.html#:~:text=In%20essence%2C%20HttpClient%20combines%20the,over%20the%20request%2Fresponse%20object.
 
    class Program
    {
        //The file name will start from avvdat-{xxxx} followed by 4 digit version
        public const string ZipFileName = "avvdat-";
        public const string ExtensionOfZipFile = ".zip";
        public static string downloadURL = "https://download.nai.com/products/commonupdater/current/vscandat1000/dat/0000/";
        //https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
                var result = await httpClient.GetStringAsync(downloadURL);
                var zipFileToDownload = result;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);
                var checkIfTheDefinitionFileExists =  doc.DocumentNode.InnerText.Contains(ZipFileName);
                if (!doc.DocumentNode.InnerText.Contains(ZipFileName)) return;

                var virusDefinitionFiles = new List<string>();

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes($"//*[contains(text(),{ZipFileName})]"))
                {
                    if (node.InnerText.ToLower().Contains(ZipFileName) &&
                        node.InnerText.ToLower().EndsWith(ExtensionOfZipFile))
                    {
                        virusDefinitionFiles.Add(node.InnerText);
                        //break;
                    }

                }
            var latestDefinitionFileName1 = virusDefinitionFiles.OrderByDescending(file => file).FirstOrDefault();
            var latestDefinitionFileName = virusDefinitionFiles.Select(x => x.Split('-')[1]).OrderByDescending(file=> file).FirstOrDefault();
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(downloadURL+ latestDefinitionFileName))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (Stream zip = File.OpenWrite($@"C:\temp\{latestDefinitionFileName}"))
                        {
                            stream.CopyTo(zip);
                        }
                    }
                   // ZipFile.CreateFromDirectory($@"C:\temp\{download}", @"C:\temp\extract");
                    ZipFile.ExtractToDirectory($@"C:\temp\{latestDefinitionFileName}", @"C:\temp\McAfeeAntivirus");
                   // ZipFile.
                }
            }catch(Exception ed)
            {
                Console.WriteLine(ed);
            }
            //var definitionFileDownload = definitionVersionsAvailable.OrderByDescending(x => x);
            //httpClient.
            
        }

        //private void test()
        //{

        //    ZipFile.CreateFromDirectory("", "");
        //}
    }
}

