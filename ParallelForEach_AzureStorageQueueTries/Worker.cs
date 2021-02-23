using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ParallelForEach_AzureStorageQueueTries
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private System.Timers.Timer timer;
        private readonly ConcurrentDictionary<string, string> _queueMessages = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentBag<string> _toBeProcessedMessages = new ConcurrentBag<string>();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //timer = new System.Timers.Timer(10*1000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
            return base.StartAsync(cancellationToken);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            Parallel.ForEach(_queueMessages, new ParallelOptions() { MaxDegreeOfParallelism = 15 }, async ap =>
            {
                try
                {
                    _logger.LogInformation($"Dictionary count::{_queueMessages.Count} -- Key::{ap.Key}");
                    if(_queueMessages.Count == 40)
                        throw new Exception("Manual EXCEPTION!!!!!!!!!!");
                    await Task.Delay(30*1000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,"Inside Time_elapsed exception", DateTimeOffset.Now);
                }
                finally
                {
                    var removed =_queueMessages.TryRemove(ap.Key, out _);
                    _logger.LogInformation($"Removed [{removed}]  Key:: {ap.Key}");
                    timer.Start();
                }
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation($"Dictionary count in ExecuteAsync::{_queueMessages.Count}");
                //var guid = Guid.NewGuid().ToString();
                //if (_queueMessages.Count < 100)
                //    _queueMessages.TryAdd(guid, $"Added this string ${guid}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
