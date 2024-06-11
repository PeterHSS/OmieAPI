using OmieAPI;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddTransient<AcessoDados>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
