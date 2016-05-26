using StatsdClient;

namespace VinculacionBackend
{
    public static class StatsdConfig
    {
        public static void Configure()
        {
            var metricsConfig = new MetricsConfig
            {
                StatsdServerName = "127.0.0.1", // If you're running the Agent locally, use this address
                Prefix = "DogstatsdDemo" // Optional; by default no prefix will be prepended
            };
            StatsdClient.Metrics.Configure(metricsConfig);
        }
    }
}