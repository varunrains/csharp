using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;

namespace ParallelForEach_AzureStorageQueueTries
{
    public class QueueService3
    {
        public void ReadFromAzureStorageQueue3([QueueTrigger("%Queue3%")] CloudQueueMessage msg)
        {

        }
    }
}