using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// This behavior controls each projectile in the game
    /// </summary>
    [DataContract]
    public class ProjectileManager : Behavior
    {
        private const int InitProjectilesCount = 40;
        private const int ProjectileCountIncrement = 8;

        public int Capacity = 0;
        public List<ProjectileController> BusyProjectiles;
        public List<ProjectileController> FreeProjectiles;

        /// <summary>
        /// Instantiate a new projectile manager
        /// </summary>
        /// 
        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.BusyProjectiles = new List<ProjectileController>();
            this.FreeProjectiles = new List<ProjectileController>();
        }

        /// <summary>
        /// Initializes the new projectile manager
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.InstantiateProjectiles(InitProjectilesCount);
        }

        /// <summary>
        /// Instantiate projectiles
        /// </summary>
        /// <param name="numProjectiles">The num projectiles</param>
        private void InstantiateProjectiles(int numProjectiles)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                ProjectileController projectile = new ProjectileController();                
                this.FreeProjectiles.Add(projectile);
            }

            this.Capacity += numProjectiles;
        }

        /// <summary>
        /// Get a new free projectile
        /// </summary>
        /// <returns>The free projectile</returns>
        public ProjectileController GetFreeProjectile()
        {
            if (this.FreeProjectiles.Count == 0)
            {
                this.InstantiateProjectiles(ProjectileCountIncrement);
            }

            ProjectileController projectile = this.FreeProjectiles[0];
            this.FreeProjectiles.RemoveAt(0);
            this.BusyProjectiles.Add(projectile);            

            return projectile;
        }

        /// <summary>
        /// Release a projectile to reuse other time
        /// </summary>
        /// <param name="projectile">The projectile</param>
        public void FreeProjectile(ProjectileController projectile)
        {
            if (this.BusyProjectiles.Remove(projectile))
            {
                this.FreeProjectiles.Add(projectile);
            }
        }

        /// <summary>
        /// Controls each projectile life
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        protected override void Update(TimeSpan gameTime)
        {
            double seconds = gameTime.TotalSeconds;

            for (int i = this.BusyProjectiles.Count - 1;  i >= 0; i--)
            {
                var projectile = this.BusyProjectiles[i];

                if (projectile.LifeTime <= 0)
                {
                    this.FreeProjectile(projectile);
                }

                projectile.Position.X = (float)(projectile.Position.X + projectile.Velocity.X * seconds);
                projectile.Position.Y = (float)(projectile.Position.Y + projectile.Velocity.Y * seconds);
                projectile.Position.Z = (float)(projectile.Position.Z + projectile.Velocity.Z * seconds);

                projectile.LifeTime -= gameTime.TotalSeconds;
            }
        }
    }
}
