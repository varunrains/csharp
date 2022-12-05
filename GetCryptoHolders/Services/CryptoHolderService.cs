using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using HtmlAgilityPack;
using GetCryptoHolders.Dtos;
using System.Text;
using System.Threading;
using PuppeteerSharp;

namespace GetCryptoHolders.Services
{
    public class CryptoHolderService
    {
        public static Dictionary<string, string> TokenSymbolAndAddress = new Dictionary<string, string>();
        public static List<string> BcsScanTokens = new List<string>() { "EOS","EGLD", "DOT", "ADA","ETH","XRP", "DOGE","LTC","BCH","ATOM","CAKE","TRX","XTZ","ETC","FIL" };
        public CryptoHolderService()
        {
            TokenSymbolAndAddress.Add("ADA", "0x3ee2200efb3400fabb9aacf31297cbdd1d435d47");
            //TokenSymbolAndAddress.Add("MATIC", "0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0");
            //TokenSymbolAndAddress.Add("HEX",  "0x2b591e99afe9f32eaa6214f7b7629768c40eeb39");
            //TokenSymbolAndAddress.Add("1INCH","0x111111111117dc0aa78b770fa6a738034120c302");
            //TokenSymbolAndAddress.Add("DOT", "0x7083609fce4d1d8dc0c979aab8c869ea2c873402");
            //TokenSymbolAndAddress.Add("UNI", "0x1f9840a85d5af5bf1d1762f925bdaddc4201f984");
            //TokenSymbolAndAddress.Add("ETH", "0x2170ed0880ac9a755fd29b2688956bd959f933f8");
            //TokenSymbolAndAddress.Add("XRP", "0x1d2f0da169ceb9fc7b3144628db156f3f6c60dbe");
            //TokenSymbolAndAddress.Add("DOGE","0xba2ae424d960c26247dd6c32edc70b295c744c43");
            //TokenSymbolAndAddress.Add("LINK", "0x514910771af9ca656af840dff83e8264ecf986ca");
            //TokenSymbolAndAddress.Add("LTC", "0x4338665cbb7b2485a8855a139b75d5e34ab0db94");
            //TokenSymbolAndAddress.Add("BCH", "0x8fF795a6F4D97E7887C79beA79aba5cc76444aDf");
            //TokenSymbolAndAddress.Add("ATOM", "0x0eb3a705fc54725037cc9e008bdede697f62f335");
            //TokenSymbolAndAddress.Add("WBTC", "0x2260fac5e5542a773aa44fbcfedf7c193bc2c599");
            //TokenSymbolAndAddress.Add("CAKE", "0x0e09fabb73bd3ade0a17ecc321fd13a19e81ce82");
            //TokenSymbolAndAddress.Add("FTT", "0x50d1c9771902476076ecfc8b2a83ad6b9355a4c9");
            //TokenSymbolAndAddress.Add("TRX", "0x85eac5ac2f758618dfa09bdbe0cf174e7d574d5b");
            //TokenSymbolAndAddress.Add("CETH", "0x4ddc2d193948926d02f9b1fe9e1daa0718270ed5");
            //TokenSymbolAndAddress.Add("XTZ", "0x16939ef78684453bfdfb47825f8a5f714f12623a");
            //TokenSymbolAndAddress.Add("ETC", "0x3d6545b08693dae087e957cb1180ee38b9e3c25e");
            //TokenSymbolAndAddress.Add("FIL", "0x0d8ce2a99bb6e3b7db580ed848240e4a0f9ae153");
            //TokenSymbolAndAddress.Add("CDAI", "0x5d3a536e4d6dbd6114cc1ead35777bab948e3643");
            //TokenSymbolAndAddress.Add("OKB", "0x75231f58b43240c9718dd58b4967c5114342a86c");
            //TokenSymbolAndAddress.Add("SHIB", "0x95ad61b0a150d79219dcf64e1e6cc01f0b64c4ce");
            //TokenSymbolAndAddress.Add("AXS", "0xbb0e17ef65f82ab018d8edd776e8dd940327b28b");
            //TokenSymbolAndAddress.Add("EGLD", "0xbf7c81fff98bbe61b40ed186e4afd6ddd01337fe");
            //TokenSymbolAndAddress.Add("EOS", "0x56b6fb708fc5732dec1afc8d8556423a2edccbd6");
        }

        public async Task GetHolders1()
        {
            string fullUrl = "https://etherscan.io/token/0x7d1afa7b718fb893db30a3abc0cfc608aacfebb0#balances";

            List<string> programmerLinks = new List<string>();

            var options = new LaunchOptions()
            {
                Headless = false,
                ExecutablePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"
            };
            //var browser = await Puppeteer.LaunchAsync(options, null);
            //var page = await browser.NewPageAsync();
            //await page.GoToAsync(fullUrl);
            //var links = @"Array.from(document.querySelectorAll('a')).map(a => a.href);";
            //var urls = await page.EvaluateExpressionAsync<string[]>(links);

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                var navigation = new NavigationOptions
                {
                    Timeout = 0,
                    WaitUntil = new[] {
                        WaitUntilNavigation.DOMContentLoaded }
                };
                await page.GoToAsync(fullUrl, navigation);
                // Type into search box.
                //await page.TypeAsync("#searchbox input", "Headless Chrome");

                // Wait for suggest overlay to appear and click "show all results".
               // var allResultsSelector = ".table table-md-text-normal table-hover";
                var allResultsSelector = "#ContentPlaceHolder1_tabHolders";
                await page.WaitForSelectorAsync(allResultsSelector);
               // await page.ClickAsync(allResultsSelector);
               // Thread.Sleep(10 * 1000);
                var ds = await page.GetContentAsync();
                // continue the operation 
            }


            //foreach (string url in urls)
            //{
            //    programmerLinks.Add(url);
            //}
        }


        public async Task GetHolders()
        {
            //https://bscscan.com/token/0x111111111117dc0aa78b770fa6a738034120c302
            //string baseUrl = "https://etherscan.io/token/";// 0x2b591e99afe9f32eaa6214f7b7629768c40eeb39#balances"
            //var restClient = new RestClient(baseUrl);

            foreach (var token in TokenSymbolAndAddress)
            {
                try
                {
                    //string baseUrl = BcsScanTokens.Contains(token.Key) ? "https://bscscan.com/token/" : "https://etherscan.io/token/";

                    var baseUrl = "https://etherscan.io/token/generic-tokenholders2?m=normal&a=0x7d1afa7b718fb893db30a3abc0cfc608aacfebb0&p=1";
                    var restClient = new RestClient(baseUrl);

                    var request = new RestRequest()
                    {
                        //Resource = $"{token.Value}#balances"
                    };

                     var response = await restClient.ExecuteTaskAsync(request).ConfigureAwait(false);
                    //var response =  restClient.ExecuteAsGet(request,"GET");
                    // restClient.
                    //var responses = restClient.DownloadData(request);
                    //var st = Encoding.UTF8.GetString(responses);
                   // EventWaitHandle Wait = new AutoResetEvent(false);

                    //restClient.ExecuteAsync(request, response =>
                    //{
                    //    Thread.Sleep(20 * 1000);
                    //    if (response.ResponseStatus == ResponseStatus.Completed)
                    //    {
                    //        RestResponse resource = (RestResponse)response;
                    //        string content = resource.Content;
                    //        //resp = Convert.ToBoolean(JsonHelper.FromJson<string>(content));
                    //        Wait.Set();
                    //    }
                    //});

                    //Wait.WaitOne();
                    //var ds = response;
                    ParseTheAverageHoldingOfTopTenWalletHolders(response, token.Key);
                    //if (BcsScanTokens.Contains(token.Key))
                    //{
                    //    ParseTheWebSiteToGetHoldersFromBscScan(response, token.Key);
                    //}
                    //else { ParseTheWebSiteToGetHoldersFromEtherScan(response, token.Key); }
                }
                catch (Exception)
                {

                }
            }

        }

        public void ParseTheAverageHoldingOfTopTenWalletHolders(IRestResponse response, string tokenName)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response.Content.ToString());

            string xpathExpression = "//div[contains(@id, 'maintable')]//tr//td";

            var ds = doc.DocumentNode.SelectNodes("//div[contains(@id, 'maintable')]");

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpathExpression))
            {
                var endIndex = node;



                //var holderValue = node.InnerText.Trim().Substring(0, endIndex - 1).Trim().Replace(",", "");
                //UploadDetailsToFireBase(tokenName, holderValue);
            }
        }

        public void ParseTheWebSiteToGetHoldersFromBscScan(IRestResponse response, string tokenName)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response.Content.ToString());

            string xpathExpression = "//div[contains(@id, 'ContentPlaceHolder1_tr_tokenHolders')]//*[contains(@class, 'mr-3')]";


            foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpathExpression))
            {
                int endIndex = node.InnerText.IndexOf("address");

                var holderValue = node.InnerText.Trim().Substring(0, endIndex - 1).Trim().Replace(",", "");
                UploadDetailsToFireBase(tokenName, holderValue);
            }
        }

        public void ParseTheWebSiteToGetHoldersFromEtherScan(IRestResponse response, string tokenName)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response.Content.ToString());

            //https://stackoverflow.com/questions/15930683/click-a-button-with-xpath-containing-partial-id-and-title-in-selenium-ide
            string xpathExpression = "//div[contains(@id, 'ContentPlaceHolder1_tr_tokenHolders')]//*[contains(@class, 'mr-3')]";


            foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpathExpression))
            {
                int endIndex = node.InnerText.IndexOf("(");

                var holderValue = node.InnerText.Trim().Substring(0, endIndex - 1).Trim().Replace(",", "");
                UploadDetailsToFireBase(tokenName, holderValue);
            }
        }

        public void UploadDetailsToFireBase(string tokenName, string numberOfHolders)
        {
            var postObject = new HolderDto()
            {
                Date = DateTime.Now.Date.ToString("MM-dd-yyyy"),
                TokenName = tokenName,
                NumberOfTokenHolders = numberOfHolders
            };

            string baseUrl = "https://angulartestingmaxmiller.firebaseio.com/";// 0x2b591e99afe9f32eaa6214f7b7629768c40eeb39#balances"
            var restClient = new RestClient(baseUrl);
            
            var request = new RestRequest()
            {
                Resource = $"holders.json",
                RequestFormat = DataFormat.Json,
            };

            request.AddJsonBody(postObject);

            restClient.ExecuteAsPost(request, "POST");

        }
    }
}
