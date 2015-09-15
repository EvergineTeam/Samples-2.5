#region File Description
//-----------------------------------------------------------------------------
// ExplosionBehavior
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using System;
using WaveEngine.Framework;
using WaveEngine.Components.Particles;

namespace Kinect2DGameSampleProject.Behaviors
{
    /// <summary>
    /// Explosion Behavior
    /// </summary>
    public class ExplosionBehavior : Behavior
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The current
        /// </summary>
        private TimeSpan current = TimeSpan.Zero;

        /// <summary>
        /// The particle system
        /// </summary>
        [RequiredComponent]
        private ParticleSystem2D particleSystem;

        /// <summary>
        /// Allows this instance to execute custom logic during its 
        /// <c>Update</c>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <remarks>
        /// This method will not be executed if the 
        /// <see cref="T:WaveEngine.Framework.Component" />, or the 
        /// <see cref="T:WaveEngine.Framework.Entity" />
        ///             owning it are not 
        /// <c>Active</c>.
        /// </remarks>
        protected override void Update(System.TimeSpan gameTime)
        {
            // Emits only by Duration Time.
            if (this.current > TimeSpan.Zero)
            {
                this.current -= gameTime;
                this.particleSystem.Emit = true;
            }
            else
            {
                this.particleSystem.Emit = false;
            }
        }

        /// <summary>
        /// Explodes this instance.
        /// </summary>
        public void Explode()
        {
            this.current = this.Duration;
        }
    }
}
