using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Behavior that filter the rotation by a specified factor
    /// </summary>
    [DataContract]
    public class HudRotationBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;

        [DataMember]
        public Vector3 RotationFactor { get; set; }


        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.RotationFactor = Vector3.One;
        }

        /// <summary>
        /// Apply the rotation factor.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.transform.Rotation *= this.RotationFactor;
        }
    }
}
