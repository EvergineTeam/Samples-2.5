using System;
using System.Collections.Generic;
using System.Text;

namespace NoesisWPFLibrary.ViewModels
{
    public abstract class BaseMissionVM
    {
        private MissionInfo missionInfo;

        public BaseMissionVM(MissionEnum mission)
        {
            this.missionInfo = MissionFactory.GetMission(mission);
        }

        public string Title
        {
            get
            {
                return this.missionInfo.Title;
            }
        }

        public string MissionType
        {
            get
            {
                return this.missionInfo.MissionType;
            }
        }

        public string LaunchDate
        {
            get
            {
                return this.missionInfo.LaunchDate.ToLongDateString();
            }
        }

        public string Mass
        {
            get
            {
                int pounds = (int)(this.missionInfo.Mass * 2.20462);
                return string.Format("{0} kilograms ({1} lb)", this.missionInfo.Mass, pounds);
            }
        }

        public string Rocket
        {
            get
            {
                return this.missionInfo.Rocket;
            }
        }

        public string MissionPlannedDuration
        {
            get
            {
                int days = (int)(1.02749125170f * this.missionInfo.MissionPlannedDurationSols);
                return string.Format("{0} sols ({1} days)", this.missionInfo.MissionPlannedDurationSols, days);
            }
        }

        public string MissionDuration
        {
            get
            {
                int days = (int)(1.02749125170f * this.missionInfo.MissionDurationSols);
                return string.Format("{0} sols ({1} days)", this.missionInfo.MissionDurationSols, days);
            }
        }

        public string Manufacturers
        {
            get
            {
                return this.missionInfo.Manufacturers;
            }
        }
    }

    public class CuriosityMission : BaseMissionVM
    {
        public CuriosityMission() : base(MissionEnum.Curiosity) { }
    }

    public class SpiritMission : BaseMissionVM
    {
        public SpiritMission() : base(MissionEnum.Spirit) { }
    }

    public class GlobalSurveyorMission : BaseMissionVM
    {
        public GlobalSurveyorMission() : base(MissionEnum.MarsGlobarSurveyor) { }
    }
}
