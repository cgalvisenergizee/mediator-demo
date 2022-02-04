using MediatR;
using Program.Data;
using Program.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers.Vehicles.Commands
{
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

            return Task.FromResult(enabledTracks.FirstOrDefault());
        }
    }
}
