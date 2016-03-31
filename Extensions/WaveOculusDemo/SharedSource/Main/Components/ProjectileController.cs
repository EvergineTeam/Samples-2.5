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
    /// Projectile setting
    /// </summary>
    public class ProjectileController
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public float VelocityLength;
        public double LifeTime = 0;
        public bool IsAlive = false;

        /// <summary>
        /// Launch a particle
        /// </summary>
        /// <param name="position">Position the particle</param>
        /// <param name="velocity">The velocity vector</param>
        /// <param name="timeLife">The expiration time of the particle</param>
        public void LaunchParticle(Vector3 position, Vector3 velocity, float timeLife)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.VelocityLength = velocity.Length();
            this.LifeTime = timeLife;
            this.IsAlive = true;
        }
    }
}
