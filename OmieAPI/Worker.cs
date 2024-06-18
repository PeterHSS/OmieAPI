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
            try
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
                    await Task.Delay(86400000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the worker.");
                throw;
            }

        }
        
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
            await base.StopAsync(cancellationToken);
        }
    }
}
