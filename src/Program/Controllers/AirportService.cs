using Microsoft.Extensions.Logging;

namespace Program.Controllers
{
    public interface IAirportService
    {
        void SmulateLandings();
    }

    public class AirportService : IAirportService
    {
        private readonly ILogger<AirportService> _logger;

        public AirportService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AirportService>();
        }

        public void SmulateLandings()
        {
            _logger.LogInformation($"Simulate landings");
        }
    }
}
