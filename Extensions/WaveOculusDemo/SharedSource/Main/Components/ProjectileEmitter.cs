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
    /// Projectile emitter behavior
    /// </summary>
    [DataContract]
    public class ProjectileEmitter : Behavior
    {
        [RequiredComponent]
        private Transform3D transform = null;
        private ProjectileManager projectileManager;

        private double delay;
        private double shootPeriod;
        private double toTheNextParticle;

        public event EventHandler OnShoot;

        [DataMember]
        public float Velocity { get; set; }

        [DataMember]
        public float ShootRate { get; set; }

        [DataMember]
        public float LifeTime { get; set; }

        [DataMember]
        public float GunFactor { get; set; }

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.Velocity = 800;
            this.ShootRate = 7f;
            this.LifeTime = 1;
            this.GunFactor = 0;
        }

        /// <summary>
        /// Initializes the projectile emitter
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.shootPeriod = (1 / this.ShootRate);
            this.delay = this.shootPeriod * this.GunFactor;
            this.toTheNextParticle = double.MaxValue;

            var projectilesEntity = this.EntityManager.Find("Projectiles");
            if (projectilesEntity != null)
            {
                this.projectileManager = projectilesEntity.FindComponent<ProjectileManager>();
            }
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
            if (this.projectileManager != null)
            {
                var projectile = this.projectileManager.GetFreeProjectile();

                var position = this.transform.Position;
                var forward = this.transform.WorldTransform.Forward;
                forward.Normalize();
                Vector3 projectileVelocity = forward * this.Velocity;


                projectile.LaunchParticle(position, projectileVelocity, this.LifeTime);

                if (this.OnShoot != null)
                {
                    this.OnShoot(this, null);
                }
            }
        }
    }
}
