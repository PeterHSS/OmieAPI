using OmieAPI.Dados;
using OmieAPI.Negocio;

namespace OmieAPI
{
    public class Worker(ILogger<Worker> logger, AcessoDados acessoDados) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly AcessoDados _acessoDados = acessoDados;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        using (OmieAPINegocio negocio = new(_acessoDados))
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
