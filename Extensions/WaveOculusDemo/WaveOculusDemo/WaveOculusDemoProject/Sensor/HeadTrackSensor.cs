using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace WaveOculusDemoProject.Sensor
{
    public class HeadTrackSensor : Service, IHeadTrackSensor
    {
        Input input;

        public bool IsSupported
        {
            get
            {
                return input.MotionState.IsConnected;
            }
        }

        public HeadTrackSensor()
        {
            this.input = WaveServices.Input;
        }

        public Quaternion GetHeadOrientation()
        {
            return this.input.MotionState.Orientation;
        }

        public Vector3 GetHeadPosition()
        {
            return Vector3.Zero;
        }

        protected override void Initialize()
        {
            this.input.StartMotion();
        }

        protected override void Terminate()
        {
            this.input.StopMotion();
        }
    }
}
