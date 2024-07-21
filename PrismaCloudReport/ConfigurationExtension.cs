using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace PrismaCloudReport
{
    public static class ConfigurationExtension
    {
        public static void ConfigureKeyVault(this IConfigurationBuilder config)
        {
            var buildConfig = config.Build();
            var vaultUri = buildConfig.GetValue<string>("VaultUri");
            config.AddAzureKeyVault(new Uri(vaultUri), new DefaultAzureCredential());
        }
    }
}
