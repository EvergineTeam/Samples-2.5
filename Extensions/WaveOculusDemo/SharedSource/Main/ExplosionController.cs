using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Sound;
using WaveOculusDemoProject.Components;

namespace WaveOculusDemo
{
    [DataContract]
    public class ExplosionController : Component
    {
        private List<ParticleSystem3D> particleSystems;
        private ShockwaveBehavior shockwave;

        [DataMember]
        public int Frame { get; set; }

        [RequiredComponent]
        private SoundEmitter3D emitter;

        protected override void DefaultValues()
        {
            base.DefaultValues();
            this.particleSystems = new List<ParticleSystem3D>();
            this.Frame = 2556;
        }

        protected override void Initialize()
        {
            base.Initialize();

            foreach(var particleEntity in this.Owner.FindChildrenByTag("Particle"))
            {
                this.particleSystems.Add(particleEntity.FindComponent<ParticleSystem3D>());
            }

            this.shockwave = this.Owner.FindChildrenByTag("Shockwave").First().FindComponent<ShockwaveBehavior>();

            var screenplay = this.Owner.Scene.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();
            screenplay.FrameEvent(this.Frame, this.BayExplosion);

        }

        /// <summary>
        /// Start explosion
        /// </summary>
        public void BayExplosion()
        {
            foreach (var particle in this.particleSystems)
            {
                particle.Emit = true;
            }

            // TODO: this.emitter.Play(SoundType.Explosion, 3, false);
            this.shockwave.StartShockWave();

            this.emitter.Play();

            WaveServices.TimerFactory.CreateTimer("StopEmit", TimeSpan.FromSeconds(0.1), this.StopEmission, false);
        }

        /// <summary>
        /// Stop particle emission
        /// </summary>
        private void StopEmission()
        {
            foreach (var particle in this.particleSystems)
            {
                particle.Emit = false;
            }
        }
    }
}
