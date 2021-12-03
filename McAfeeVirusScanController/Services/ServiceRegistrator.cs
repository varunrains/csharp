using Microsoft.Extensions.DependencyInjection;
using VirusScanApi.Services;

namespace McAfeeVirusScanController.Services
{
    public static class ServicesRegistrator
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<VirusScanService>();
            services.AddScoped<CommandLineScannerService>();
            services.AddScoped<FileSystemService>();
        }
    }
}
