using SisandBackend;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false, true)
#if DEBUG
                 .AddJsonFile("appsettings.Development.json", true, true)
#else
                 .AddJsonFile("appsettings.Production.json", true, true)
#endif
                 .Build();


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
                    .UseIISIntegration()
                    .ConfigureKestrel(options => options.ListenAnyIP(5000));
            });
}
