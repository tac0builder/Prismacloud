using Microsoft.Extensions.Hosting;

namespace PrismaCloudReport
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(config => config.ConfigureKeyVault())
                .Build();

            host.Run();
        }
    }
}