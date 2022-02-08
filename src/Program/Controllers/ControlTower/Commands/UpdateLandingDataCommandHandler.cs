using MediatR;
using Program.Data;
using Program.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers.ControlTower.Commands
{
    /// <summary>
    /// MediatR Handler for release a landing track
    /// </summary>
    public class UpdateLandingDataCommandHandler : IRequestHandler<UpdateLandingDataCommand, bool>
    {
        private readonly IRepository<Airport> _airportRepository;

        public UpdateLandingDataCommandHandler(IRepository<Airport> airportRepository)
        {
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

            // Landing track usage time
            var rand = new Random();
            await Task.Delay(rand.Next(2000, 5000));

            if (selectedTrack != null)
                selectedTrack.Busy = false;

            return !selectedTrack?.Busy ?? false;
        }
    }
}
