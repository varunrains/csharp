using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;

namespace ParallelForEach_AzureStorageQueueTries
{
    public class QueueService4
    {
        public void ReadFromAzureStorageQueue4([QueueTrigger("%Queue4%")] CloudQueueMessage msg)
        {

        }
    }
}