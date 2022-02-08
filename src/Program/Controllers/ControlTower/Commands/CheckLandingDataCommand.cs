using MediatR;
using Program.Models;

namespace Program.Controllers.ControlTower.Commands
{
    public class CheckLandingDataCommand : IRequest<LandingTrack>
    { }
}
