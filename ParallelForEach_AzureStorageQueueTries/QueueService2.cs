using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ParallelForEach_AzureStorageQueueTries
{
    public class QueueService2: QueueService
    {
        private System.Timers.Timer _timer;
        private readonly ILogger<QueueService1> _logger;
        public QueueService2(ILogger<QueueService1> logger) : base(logger)
        {
            _logger = logger;
            base.StartTimer(_timer);
        }
        //public static void Elapsed(object sender, ElapsedEventArgs e)
        //{

        //}

        public void ReadFromAzureStorageQueue2([QueueTrigger("%queue2%")] CloudQueueMessage msg)
        {
            AddMessagesToQueue(msg);
        }

    }
}

