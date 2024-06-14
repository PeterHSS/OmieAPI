namespace OmieAPI
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AcessoDados _acessoDados;


        public Worker(ILogger<Worker> logger, AcessoDados acessoDados)
        {
            _logger = logger;
            _acessoDados = acessoDados;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    using (Negocio negocio = new(_acessoDados))
                    {
                        await negocio.ExecutaLogica();
                    };
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
