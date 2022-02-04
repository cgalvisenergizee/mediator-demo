﻿using System.Collections.Generic;

namespace Program.Models
{
    public class Airport
    {
        public string Name { get; set; }
        public List<LandingTrack> LandingTracks { get; set; }
        public List<ControlTower> ControlTowers { get; set; }
    }

    public class LandingTrack
    {
        public string Name { get; set; }
    }
}
