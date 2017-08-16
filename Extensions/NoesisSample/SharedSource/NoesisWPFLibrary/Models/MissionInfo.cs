using System;
using System.Collections.Generic;
using System.Text;

namespace NoesisWPFLibrary
{
    public class MissionInfo
    {
        public string Title { get; set; }

        public string MissionType { get; set; }

        public DateTime LaunchDate { get; set; }

        public int Mass { get; set; }

        public string Rocket { get; set; }

        public int MissionPlannedDurationSols { get; set; }

        public int MissionDurationSols { get; set; }

        public string Manufacturers { get; set;}
    }
}
