using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;

namespace WaveOculusDemoProject.Sensor
{
    public interface IHeadTrackSensor
    {
        /// <summary>
        /// Gets a value indicating if head track sensor is supported in this device.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Get head orientation
        /// </summary>
        Quaternion GetHeadOrientation();

        /// <summary>
        /// Get head position. This is useful in DK2 model.
        /// </summary>
        Vector3 GetHeadPosition();
    }
}
