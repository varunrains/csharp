using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharepointToAzureCore.Configuration;
using SharepointToAzureCore.DB;

namespace SharepointToAzureCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
           
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XmlConfigurator.Configure(repository,new FileInfo(Path.Combine(assemblyFolder, "log4net.config")));
            CreateHostBuilder(args).Build().Run();
           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<SodiumRepository>();
                    services.AddSingleton<SharePointOnlineHelper>();
                    services.AddSingleton<SharePointOnPremHelper>();
                    services.AddSingleton<AzureBlobHelper>();
                    //services.AddSingleton<MemoryCacheContext>();
                    services.Configure<BptConfiguration>(options => hostContext.Configuration.GetSection("BConfiguration").Bind(options));
                });
    }
}
