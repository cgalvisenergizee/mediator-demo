using MediatR;
using Program.Models;

namespace Program.Controllers.Vehicles.Commands
{
    public class CheckLandingDataCommand : IRequest<LandingTrack>
    { }
}
