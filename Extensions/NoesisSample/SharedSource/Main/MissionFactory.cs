using NoesisSample.ViewModels;
using System;

namespace NoesisSample
{
    public class MissionFactory
    {
        public static MissionInfo CuriosityMission
        {
            get
            {
                return new MissionInfo()
                {
                    Title = "Curiosity Rover",
                    MissionType = "Exploration rover vehicle",
                    LaunchDate = new DateTime(2011, 11, 26),
                    Mass = 899,
                    Manufacturers = "JPL, Boeing,Lockheed Martin",
                    Rocket = "Atlas V 541",
                    MissionPlannedDurationSols = 668,
                    MissionDurationSols = 1759
                };
            }
        }

        public static MissionInfo SpiritMission
        {
            get
            {
                return new MissionInfo()
                {
                    Title = "Spirit Rover",
                    MissionType = "Rover",
                    LaunchDate = new DateTime(2003, 6, 10),
                    Mass = 185,
                    Manufacturers = "JPL",
                    Rocket = "Delta II 7925-9.5",
                    MissionPlannedDurationSols = 90,
                    MissionDurationSols = 2269
                };
            }
        }

        public static MissionInfo GlobalSurveyorMission
        {
            get
            {
                return new MissionInfo()
                {
                    Title = "Mars Global Surveyor",
                    MissionType = "Mars Orbiter",
                    LaunchDate = new DateTime(1996, 11, 7),
                    Mass = 1030,
                    Manufacturers = "JPL",
                    Rocket = "	Delta II 7925",
                    MissionPlannedDurationSols = 651,
                    MissionDurationSols = 3249
                };
            }
        }

        public static MissionInfo GetMission(MissionEnum mission)
        {
            switch (mission)
            {
                case MissionEnum.Curiosity:
                    return CuriosityMission;
                case MissionEnum.Spirit:
                    return SpiritMission;
                case MissionEnum.MarsGlobarSurveyor:
                    return GlobalSurveyorMission;
                default:
                    return null;
            }
        }

        public static BaseMissionVM GetMissionVM(MissionEnum mission)
        {
            switch (mission)
            {
                case MissionEnum.Curiosity:
                    return new CuriosityMission();
                case MissionEnum.Spirit:
                    return new SpiritMission();
                case MissionEnum.MarsGlobarSurveyor:
                    return new GlobalSurveyorMission();
                default:
                    return null;
            }
        }
    }
}
