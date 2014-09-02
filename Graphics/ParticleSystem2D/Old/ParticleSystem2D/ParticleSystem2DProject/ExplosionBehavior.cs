using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace ParticleSystem2DProject
{
    public class ExplosionBehavior : Behavior
    {
        private const double EXPLOSIONSECONDS = 0.1;

        [RequiredComponent]
        public Transform2D transform;

        public ParticleSystem2D particleSystem;
        public ParticleSystem2D dinos;

        private double explosionLeft = EXPLOSIONSECONDS;

        public ExplosionBehavior()
            : base("explosionBehavior")
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.particleSystem = this.Owner.FindChild("explosionParticles").FindComponent<ParticleSystem2D>();
            this.particleSystem.Emit = false;

            this.dinos = this.Owner.FindChild("dinos").FindComponent<ParticleSystem2D>();
            this.dinos.Emit = false;
        }

        public void Explode(float x, float y)
        {
            if (this.particleSystem == null)
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
            if (this.particleSystem.Emit)
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
