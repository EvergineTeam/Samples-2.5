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
    /// Projectile emitter behavior
    /// </summary>
    public class ProjectileEmitter : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;
        private ProjectileManager projectileManager;

        private float velocity;
        private float shootRate;
        private double delay;
        private double shootPeriod;
        private float lifeTime;
        private double toTheNextParticle;

        public event EventHandler OnShoot;
       
        /// <summary>
        /// Creates a new projectile emitter behavior
        /// </summary>
        /// <param name="velocity">Projectile absolute velocity</param>
        /// <param name="shootRate">Shoot rate of projectiles</param>
        /// <param name="lifeTime">Projectile life time</param>
        /// <param name="gunFactor">Time offset used to shoot particles</param>
        public ProjectileEmitter(float velocity, float shootRate, float lifeTime, float gunFactor)
        {            
            this.velocity = velocity;
            this.shootRate = shootRate;
            this.shootPeriod = (1 / shootRate);
            this.delay = this.shootPeriod * gunFactor;            
            this.lifeTime = lifeTime;
            this.toTheNextParticle = double.MaxValue;
        }

        /// <summary>
        /// Initializes the projectile emitter
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.projectileManager = this.EntityManager.Find("Projectiles").FindComponent<ProjectileManager>();
        }

        /// <summary>
        /// Star firing particles
        /// </summary>
        public void StarFiring()
        {
            this.toTheNextParticle = this.delay;
            this.IsActive = true;
        }

        /// <summary>
        /// Stop firing particles
        /// </summary>
        public void StopFiring()
        {
            this.IsActive = false;
        }

        /// <summary>
        /// Update particle emitter
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (this.toTheNextParticle <= 0)
            {
                this.ShootProjectile();
                this.toTheNextParticle = this.shootPeriod;
            }

            this.toTheNextParticle -= gameTime.TotalSeconds;
        }

        /// <summary>
        /// Shot a single projectile
        /// </summary>
        private void ShootProjectile()
        {
            var projectile = this.projectileManager.GetFreeProjectile();

            var position = this.transform.Position;
            var forward = this.transform.WorldTransform.Forward;
            forward.Normalize();
            Vector3 projectileVelocity = forward * this.velocity;


            projectile.LaunchParticle(position, projectileVelocity, this.lifeTime);

            if (this.OnShoot != null)
            {
                this.OnShoot(this, null);
            }
        }
    }
}
