using log4net.Config;
using log4net.Repository;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Unity;

namespace SharepointToAzure
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());

            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XmlConfigurator.Configure(repository, new FileInfo(Path.Combine(assemblyFolder, "log4net.config")));

            UnityContainer container = new UnityContainer();
            var worker = container.Resolve<Worker>();
            await worker.ExecuteAsync().ConfigureAwait(false);
        }




    }
}
