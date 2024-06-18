using OmieAPI;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
     .ConfigureLogging(logging =>
     {
         logging.ClearProviders();
         logging.AddConsole();
         logging.AddEventLog(); 
     })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<AcessoDados>();
        services.AddHostedService<Worker>();
    })
    .UseWindowsService();

var host = builder.Build();

host.Run();
