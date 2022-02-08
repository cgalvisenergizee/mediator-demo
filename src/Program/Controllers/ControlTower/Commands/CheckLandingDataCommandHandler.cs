using MediatR;
using Program.Data;
using Program.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers.ControlTower.Commands
{
    /// <summary>
    /// MediatR Handler for get a landing track available
    /// </summary>
    public class CheckLandingDataCommandHandler : IRequestHandler<CheckLandingDataCommand, LandingTrack>
    {
        private readonly IRepository<Airport> _airportRepository;

        public CheckLandingDataCommandHandler(IRepository<Airport> airportRepository)
        {
            _airportRepository = airportRepository;
        }

        public Task<LandingTrack> Handle(CheckLandingDataCommand request, CancellationToken cancellationToken)
        {
            var airport = _airportRepository
                .GetAll()
                .FirstOrDefault();

            var enabledTracks = airport.LandingTracks
                .Where(t => !t.Busy);

            var enabledTrack = enabledTracks.FirstOrDefault();

            if (enabledTrack != null)
                enabledTrack.Busy = true;

            return Task.FromResult(enabledTrack);
        }
    }
}
