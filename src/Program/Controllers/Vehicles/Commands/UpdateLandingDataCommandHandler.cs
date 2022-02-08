using MediatR;
using Microsoft.Extensions.Logging;
using Program.Data;
using Program.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers.Vehicles.Commands
{
    public class UpdateLandingDataCommandHandler : IRequestHandler<UpdateLandingDataCommand, bool>
    {
        private readonly IRepository<Airport> _airportRepository;
        private readonly ILogger<UpdateLandingDataCommandHandler> _logger;

        public UpdateLandingDataCommandHandler(ILoggerFactory loggerFactory, 
            IRepository<Airport> airportRepository)
        {
            _logger = loggerFactory.CreateLogger<UpdateLandingDataCommandHandler>();
            _airportRepository = airportRepository;
        }

        public async Task<bool> Handle(UpdateLandingDataCommand request, CancellationToken cancellationToken)
        {
            var airport = _airportRepository
                .GetAll()
                .FirstOrDefault();

            var selectedTrack = airport.LandingTracks
                .Where(t => t.Name == request.LandingTrack.Name)
                .FirstOrDefault();

            await Task.Delay(2000);

            if (selectedTrack != null)
                selectedTrack.Busy = false;

            _logger.LogInformation($"Track {selectedTrack.Name} released");

            return !selectedTrack?.Busy ?? false;
        }
    }
}
