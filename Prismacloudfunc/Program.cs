using Microsoft.Extensions.Hosting;

namespace PrismaCloudFunc
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