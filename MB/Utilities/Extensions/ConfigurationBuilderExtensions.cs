using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Diagnostics;

namespace MB.Utilities.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        private const string KeyVaultName = "KeyVaultName";
        private const string KeyVaultClientId = "KeyVaultClientId";
        private const string KeyVaultClientSecret = "KeyVaultClientSecret";

        /// <summary>
        /// Our code needs different configuration in different run environments.
        /// We recognize 3 distinct situations that affect the way configuration is obtained:
        /// - ran locally on a development machine or in a unit test
        /// - ran from the application at a release environment
        /// - ran from the integrationtests or release pipeline
        ///
        /// When the application is run in a release environment, it has a ManagedIdentity.
        /// The AzureServiceTokenProvider will use the ManagedIdentity to obtain an accesstoken for keyvault.
        ///
        /// The process that runs the integration tests (from pipeline) has no such ManagedIdentity.
        /// Instead, we assume it has a (manually created) ServicePrincipal within the Active Directory.
        /// We inject the ServicePrincipal's credentials from the pipeline as environment variables.
        ///
        ///
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            var configuration = configurationBuilder.Build();

            if (!IsLocalDevelopment())
            {
                var keyVaultName = configuration[KeyVaultName];
                if (!string.IsNullOrWhiteSpace(keyVaultName))
                {
                    Trace.WriteLine($"Using '{keyVaultName}' as Key Vault.");

                    var keyVaultClientId = configuration[KeyVaultClientId];
                    var keyVaultClientSecret = configuration[KeyVaultClientSecret];
                    if (!string.IsNullOrWhiteSpace(keyVaultClientId) && !string.IsNullOrWhiteSpace(keyVaultClientSecret))
                    {
                        // when run from pipeline/integrationtest: connect using the ServicePrincipal's credentials that were provided as environment variables
                        configurationBuilder.AddAzureKeyVault($"https://{keyVaultName}.vault.azure.net/", keyVaultClientId, keyVaultClientSecret);
                    }
                    else
                    {
                        // when run from the released application: get accesstoken based on the application's ManagedIdentity
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                        configurationBuilder.AddAzureKeyVault($"https://{keyVaultName}.vault.azure.net/", keyVaultClient, new DefaultKeyVaultSecretManager());
                    }
                }
            }

            return configurationBuilder;
        }

        public static bool IsLocalDevelopment()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(environmentName))
            {
                environmentName = Environment.GetEnvironmentVariable("WEBJOB_ENVIRONMENT");
            }

            return !string.IsNullOrWhiteSpace(environmentName) && environmentName.Equals("development", StringComparison.OrdinalIgnoreCase);
        }
    }
}