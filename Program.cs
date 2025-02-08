using SisandBackend;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEBUG
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
#else
            .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
#endif
            .AddEnvironmentVariables()
            .Build();

        ShowConfig(configuration);

        var builder = CreateHostBuilder(args, configuration);
        var host = builder.Build();

        host.Start();
        await host.WaitForShutdownAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddConfiguration(configuration);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseStartup<Startup>()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration();
            });

    private static void ShowConfig(IConfiguration config)
    {
        foreach (var pair in config.AsEnumerable())
        {
            Console.WriteLine($"{pair.Key} - {pair.Value}");
        }
    }
}
