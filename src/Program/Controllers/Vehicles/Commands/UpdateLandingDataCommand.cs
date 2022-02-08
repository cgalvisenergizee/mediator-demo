using MediatR;
using Program.Models;

namespace Program.Controllers.Vehicles.Commands
{
    public class UpdateLandingDataCommand : IRequest<bool>
    {
        public LandingTrack LandingTrack { get; set; }
    }
}
