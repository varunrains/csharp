using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace TaskAnyWhenAllTest
{
    class Program
    {
        //https://stackoverflow.com/questions/55495569/parallel-invoke-vs-await-async-task-performance
        //https://dev.to/scotthannen/concurrency-vs-parallel-vs-async-in-net-3812
        RestClient _client = new RestClient("https://comlimsquotationservices-ph.eurofins.local/");
        static async Task Main(string[] args)
        {
            List<string> ss = new List<string>(){ "gg", "dss" };
            string.Join(",", ss.Select(x => x));

            //var task1 = new Task(() =>
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Task 1 finished");
            //});
            //var task2 = new Task(() =>
            //{
            //    Thread.Sleep(2000);
            //    Console.WriteLine("Task 2 finished");
            //});
            //var task3 = new Task(() =>
            //{
            //    Thread.Sleep(3000);
            //    Console.WriteLine("Task 3 finished");
            //});
            //var task4 = new Task(() =>
            //{
            //    Thread.Sleep(4000);
            //    Console.WriteLine("Task 4 finished");
            //});
            //var task5 = new Task(() =>
            //{
            //    Thread.Sleep(5000);
            //    Console.WriteLine("Task 5 finished");
            //});
            Program p = new Program();
            var stopWatch = Stopwatch.StartNew();
            //var t1 = TestOfTask(1000 * 1, "t1");
            //var t2 = TestOfTask(2000 * 3, "t2");
            //var t3 = TestOfTask(2000 * 1, "t3");
            //var t4 = p.TestOfTask();
            var tasks = new List<Task>() {p.TestOfTask(), p.TestOfTask(), p.TestOfTask()};

            // p.MainParallelTest();


            //var finishedTask = await Task.WhenAny(t4).ConfigureAwait(false);

            //if (finishedTask == t1)
            //{
            //     // tasks.Remove(finishedTask);
            //     //Console.WriteLine("Inside finished task if statement;");
            //     tasks.Add(TestOfTask(2000 * 1, "t4"));

            // }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            stopWatch.Stop();
            Console.WriteLine($"Everything Done! - Time Taken: {stopWatch.ElapsedMilliseconds}");
            Console.Read();
        }

        private void MainParallelTest()
        {
            Program p = new Program();
            List<Action> actions = new List<Action>()
            {
                () => {  p.TestOfParallel();},
                () => {  p.TestOfParallel();},
                () => {  p.TestOfParallel();},
            };

            Parallel.Invoke(actions.ToArray());
        }

        private  async Task<dynamic> TestOfTask()
        {
            //await Task.Delay(delay);
            //Console.WriteLine($"Task {taskId} done!");

            Program p = new Program();
                var request = new RestRequest("Quotation/v1/QuotationItems?QuotationCode=Q7MFPH190365&QuotationVersionNumber=29", Method.GET);
                request.Credentials = new NetworkCredential()
                {
                    UserName = "US19_SVC_eLIMS MSCRM",
                    Password = "W0^mNY=wfI58t8dG`$as"
                };
                p._client.Timeout = 100000;
                //var taskCompletionResponse = new TaskCompletionSource<RestSharp.IRestResponse<ComLimsQuotationLineItemsDto>>();
                 var result = await p._client.ExecuteGetTaskAsync<dynamic>(request);
                 
                 return result.Data;
                 // var result = taskCompletionResponse.Task.GetAwaiter().GetResult();

                 //Console.WriteLine(result.Data.QuotationCode);
                 //return result.Data;
        }

        private dynamic TestOfParallel()
        {
            //await Task.Delay(delay);
            //Console.WriteLine($"Task {taskId} done!");

            Program p = new Program();
            var request = new RestRequest("Quotation/v1/QuotationItems?QuotationCode=Q7MFPH190365&QuotationVersionNumber=29", Method.GET);
            request.Credentials = new NetworkCredential()
            {
                UserName = "US19_SVC_eLIMS MSCRM",
                Password = "W0^mNY=wfI58t8dG`$as"
            };
            p._client.Timeout = 100000;
            //var taskCompletionResponse = new TaskCompletionSource<RestSharp.IRestResponse<ComLimsQuotationLineItemsDto>>();
            var result =  p._client.ExecuteGetTaskAsync<dynamic>(request).ConfigureAwait(false).GetAwaiter().GetResult();

            return result.Data;
            // var result = taskCompletionResponse.Task.GetAwaiter().GetResult();

            //Console.WriteLine(result.Data.QuotationCode);
            //return result.Data;
        }



    }
}
