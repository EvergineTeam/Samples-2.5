using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.OculusRift;

namespace WaveOculusDemoProject.Sensor
{
    public class HeadTrackSensor : Service, IHeadTrackSensor
    {
        private OVRService ovr;

        /// <summary>
        /// Gets a value indicating if this headtrack sensor is available
        /// </summary>
        public bool IsSupported
        {
            get
            {
                return this.ovr != null;
            }
        }

        public HeadTrackSensor()
        {
        }

        /// <summary>
        /// Get head orientation, expressed in quaternion.
        /// </summary>
        public Quaternion GetHeadOrientation()
        {
            if (this.ovr.IsConnected)
            {
                var q = this.ovr.GetEyePose(EyeType.Left).Orientation;
                Vector3 axis;
                float angle;
                Quaternion.ToAngleAxis(ref q, out axis, out angle);

                // flip X and Z axis to match Wave orientations.
                axis.X *= -1;
                axis.Z *= -1;

                return Quaternion.CreateFromAxisAngle(axis, angle);
            }
            else
            {
                return Quaternion.Identity;
            }
        }

        /// <summary>
        /// Get head position. This is useful in DK2 model.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public Vector3 GetHeadPosition()
        {
            Vector3 leftPosition = this.ovr.GetEyePose(EyeType.Left).Position;
            Vector3 rightPosition = this.ovr.GetEyePose(EyeType.Right).Position;

            Vector3 headPosition = Vector3.Lerp(leftPosition, rightPosition, 0.5f);
            headPosition.X *= -1;
            headPosition.Z *= -1;
            return headPosition;
        }

        protected override void Initialize()
        {
            this.ovr = WaveServices.GetService<OVRService>();
        }

        protected override void Terminate()
        {
        }
    }
}
