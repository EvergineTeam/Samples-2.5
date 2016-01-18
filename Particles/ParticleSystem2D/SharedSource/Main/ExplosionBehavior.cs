using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace ParticleSystem2DProject
{
    [DataContract]
    public class ExplosionBehavior : Behavior
    {
        private const double EXPLOSIONSECONDS = 0.1;

        [RequiredComponent]
        public Transform2D transform;

        private WaveEngine.Components.Particles.ParticleSystem2D particleSystem;
        private WaveEngine.Components.Particles.ParticleSystem2D dinos;

        private double explosionLeft;

        protected override void DefaultValues()
        {
            base.DefaultValues();

            explosionLeft = EXPLOSIONSECONDS;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var explosionParticles = this.Owner.FindChild("explosionParticles");
            if (explosionParticles != null)
            {
                this.particleSystem = explosionParticles.FindComponent<WaveEngine.Components.Particles.ParticleSystem2D>();
                if (this.particleSystem != null)
                {
                    this.particleSystem.Emit = false;
                }
            }

            var dinosEntity = this.Owner.FindChild("dinos");
            if (dinosEntity != null)
            {
                this.dinos = dinosEntity.FindComponent<WaveEngine.Components.Particles.ParticleSystem2D>();
                if (this.dinos != null)
                {
                    this.dinos.Emit = false;
                }
            }                        
        }

        public void Explode(float x, float y)
        {
            if (this.particleSystem == null || 
                this.dinos == null)
            {
                return;
            }

            this.particleSystem.Emit = true;
            this.dinos.Emit = true;
            this.explosionLeft = EXPLOSIONSECONDS;
            this.transform.X = x;
            this.transform.Y = y;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (this.particleSystem != null && this.particleSystem.Emit)
            {
                this.explosionLeft = this.explosionLeft - gameTime.TotalSeconds;
                
                if (this.explosionLeft < 0)
                {
                    this.particleSystem.Emit = false;
                    this.dinos.Emit = false;
                }
            }
        }
    }
}
