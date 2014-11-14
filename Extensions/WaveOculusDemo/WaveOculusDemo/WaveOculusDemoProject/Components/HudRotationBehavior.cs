using System;
using System.Collections.Generic;
using System.Linq;
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
    public class HudRotationBehavior : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;

        private Vector3 rotationFactor;

        /// <summary>
        /// Instantiate a new Hud rotation factor
        /// </summary>
        /// <param name="rotationFactor">The rotation factor</param>
        public HudRotationBehavior(Vector3 rotationFactor)
        {
            this.rotationFactor = rotationFactor;
        }

        /// <summary>
        /// Apply the rotation factor.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.transform.Rotation *= this.rotationFactor;
        }
    }
}
