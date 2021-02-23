using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;

namespace ParallelForEach_AzureStorageQueueTries
{
    public class QueueService5
    {
        public void ReadFromAzureStorageQueue5([QueueTrigger("%Queue5%")] CloudQueueMessage msg)
        {

        }
    }
}