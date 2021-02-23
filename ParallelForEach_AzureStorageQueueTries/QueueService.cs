using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ParallelForEach_AzureStorageQueueTries
{
    public  class QueueService
    {
        public readonly ConcurrentDictionary<string, string> _queueMessages = new ConcurrentDictionary<string, string>();
        public readonly ConcurrentBag<string> _toBeProcessedMessages = new ConcurrentBag<string>();
        private System.Timers.Timer _timer;
        private readonly ILogger<QueueService> _logger;

        protected QueueService(ILogger<QueueService> logger)
        {
            _logger = logger;
        }

        public void StartTimer(System.Timers.Timer timer)
        {
           // if(_timer !=null ) return;
            
            _timer = new System.Timers.Timer(10 * 1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Disposed += TimerOnDisposed;
            _timer.Start();
        }

        private void TimerOnDisposed(object? sender, EventArgs e)
        {
           _logger.LogInformation("Timer disposed");
        }

        //public abstract void Timer_Elapsed(object sender, ElapsedEventArgs e);
        public void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            Parallel.ForEach(_queueMessages, new ParallelOptions() { MaxDegreeOfParallelism = 15 }, async ap =>
            {
                try
                {
                    _logger.LogInformation($"Dictionary count::{_queueMessages.Count} -- Key::{ap.Key}");
                    if (_queueMessages.Count == 40)
                        throw new Exception("Manual EXCEPTION!!!!!!!!!!");
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Inside Time_elapsed 1 exception", DateTimeOffset.Now);
                }
                finally
                {
                    var removed = _queueMessages.TryRemove(ap.Key, out _);
                    AddToQueueIfThePrintJobIsDone();
                    _logger.LogInformation($"Removed [{removed}]  Key:: {ap.Key}");
                }
            });
            _timer.Start();
        }

        public void AddToQueueIfThePrintJobIsDone()
        {
            foreach (var keyValuePair in _queueMessages)
            {
                if (!_toBeProcessedMessages.Contains(keyValuePair.Key))
                {
                    _toBeProcessedMessages.Add(keyValuePair.Value);
                }
            }

            
        }

        public void AddMessagesToQueue(CloudQueueMessage msg)
        {
            var added = _queueMessages.TryAdd(msg.AsString, msg.AsString);
            if (!added)
            {
                _toBeProcessedMessages.Add(msg.AsString);
            }
        }

    }
}
