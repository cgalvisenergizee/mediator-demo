using System.Collections.Generic;

namespace Program.Models
{
    public class Airport
    {
        public string Name { get; set; }
        public List<LandingTrack> LandingTracks { get; set; } = new();
        public List<ControlTower> ControlTowers { get; set; } = new();
    }

    public class LandingTrack
    {
        public string Name { get; set; }
        public bool Busy { get; set; } = false;
    }
}
