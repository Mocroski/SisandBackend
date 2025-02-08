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


        var builder = CreateHostBuilder(args);

        var host = builder.Build();

        host.Start();

        host.WaitForShutdown();
    }
    private static IHostBuilder CreateHostBuilder(string[] args)
           => Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder
                               .UseStartup<Startup>()
                               .UseContentRoot(Directory.GetCurrentDirectory())
                               .UseIISIntegration();
                       });
}