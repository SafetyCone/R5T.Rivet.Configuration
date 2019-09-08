using System;

using Microsoft.Extensions.Configuration;


namespace R5T.Rivet.Configuration
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSecretJsonFile(this IConfigurationBuilder configurationBuilder, string jsonFileName, bool optional = false)
        {
            var secretJsonFilePath = Utilities.GetSecretsFilePath(jsonFileName);

            configurationBuilder.AddJsonFile(secretJsonFilePath, optional);

            return configurationBuilder;
        }
    }
}
