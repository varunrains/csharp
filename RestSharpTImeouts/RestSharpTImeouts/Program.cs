using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace RestSharpTImeouts
{
    class Program
    {
        RestClient _client = new RestClient();
        private string userName;
        private string password;
        static async Task Main(string[] args)
        {
            List<Action> actions = new List<Action>();
            var numberOfThreads = args.Any() ? Convert.ToInt16(args[0]) : 1;
            var timeout = args.Length == 2 ?  Convert.ToInt16(args[1]): 100000;
            //var userNameAndPasswordInfo = File.ReadAllLines(@"C:\temp\usernamepwd.txt");
            
            Program p = new Program();
            //p.userName = userNameAndPasswordInfo[0];
            //p.password = userNameAndPasswordInfo[1];

            //for (int i = 0; i < numberOfThreads; i++)
            //{
            //    int dd = i;
            //    actions.Add(() =>
            //    {
            //        //var milisecs = new Random();
            //        //Thread.Sleep(milisecs.Next(1000, 3000));
            //        ////if(dd % 2 == 0)
            //        //throw new System.NullReferenceException();
            //        if (timeout != 100000)
            //            p.MethodCheck(5000);
            //    });
            //}

            p.MethodCheck(125000);

            //int workerThreads, completionPortThreads;
            //ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            //Console.WriteLine($"Max number of worker threads : {workerThreads}");
            //Console.WriteLine($"Completion Port threads : {completionPortThreads}");

            //int minWorkerThreads, minCompletionPortThreads;
            //ThreadPool.GetMinThreads(out minWorkerThreads, out minCompletionPortThreads);
            //Console.WriteLine($"Max number of worker threads : {minWorkerThreads}");
            //Console.WriteLine($"Completion Port threads : {minCompletionPortThreads}");
            //Console.Read();

            //ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = numberOfThreads };
            //Parallel.Invoke(po, actions.ToArray());

            //await p.TaskReturn(1);

            //await p.TaskReturn(0);


        }

        private async Task TaskReturn(int valueCheck)
        {
            if(valueCheck == 0) return;

            await Task.Run(() => { Thread.Sleep(1000); });
            Console.WriteLine("I am reading now!!");
            Console.Read();

        }


        private  dynamic MethodCheck(int timeout)
        {
           
           // var request = new RestRequest("https://curl/QuotationItems?QuotationCode=Q7MFPH190365&QuotationVersionNumber=29", Method.GET);
            var request = new RestRequest("https://cURL", Method.GET);
            request.Credentials = new NetworkCredential()
           {
                Domain = "sdd",
               UserName = "USRNAME",
            
                Password = "passs"
            };
           _client.Timeout = timeout;
            var taskCompletionResponse = new TaskCompletionSource<RestSharp.IRestResponse<ComLimsQuotationLineItemsDto>>();
            var t = _client.ExecuteAsync<ComLimsQuotationLineItemsDto>(request, respone => taskCompletionResponse.SetResult(respone));
            var result = taskCompletionResponse.Task.GetAwaiter().GetResult();

            Console.WriteLine(result.Data.QuotationCode);
            Console.ReadLine();
            return result.Data;
        }


        private dynamic MethodCheck1()
        {
            RestRequest request;
            Random rand = new Random();
            request = rand.Next(2) == 1 ? new RestRequest("http://ss", Method.GET) :
             new RestRequest("http://iss", Method.GET);
           
           // Thread.Sleep(rand.Next(300));
            request.Credentials = new NetworkCredential()
            {
                UserName = userName,
                Password = password
            };
            // _client.Timeout = timeout;
            var t = _client.ExecuteGetTaskAsync<dynamic>(request);
            var result = t.GetAwaiter().GetResult();

           // Console.WriteLine(result.Data.QuotationCode);

            //var taskCompletionResponse = new TaskCompletionSource<RestSharp.IRestResponse<List<CurrencyDto>>>();
            //var t = _client.ExecuteAsync<List<CurrencyDto>>(request, respone => taskCompletionResponse.SetResult(respone));
            //var result = taskCompletionResponse.Task.GetAwaiter().GetResult();

            //var taskCompletionResponse = new TaskCompletionSource<RestSharp.IRestResponse<dynamic>>();
            //var t = _client.ExecuteAsync<dynamic>(request, respone => taskCompletionResponse.SetResult(respone));
            //var result = taskCompletionResponse.Task.GetAwaiter().GetResult();

            ////Console.WriteLine(result.Data.First());
            //return result.Data.data;
            return result;
        }




    }
}
