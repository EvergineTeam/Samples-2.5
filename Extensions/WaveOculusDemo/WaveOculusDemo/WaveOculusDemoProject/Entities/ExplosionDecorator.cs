using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveOculusDemoProject.Audio;
using WaveOculusDemoProject.Components;

namespace WaveOculusDemoProject.Entities
{
    /// <summary>
    /// Explosion entity decorator
    /// </summary>
    public class ExplosionDecorator : BaseDecorator
    {
        private Transform3D cachedTransform;
        private List<ParticleSystem3D> particleSystems = new List<ParticleSystem3D>();
        private ShockwaveBehavior shockwave;
        private Sound3DEmitter emitter;

        public Transform3D Transform3D
        {
            get { return this.cachedTransform; }
        }

        /// <summary>
        /// Creates a new explosion decorator
        /// </summary>
        /// <param name="name">The entity name</param>
        /// <param name="frame">The explosion frame </param>
        public ExplosionDecorator(string name, int frame)
        {
            this.particleSystems = new List<ParticleSystem3D>();

            this.entity = new Entity(name)
            .AddComponent(this.cachedTransform = new Transform3D())
            ;

            ParticleSystem3D particleSystem;
            var emitter1 = new Entity()
            .AddComponent(new Transform3D())
            .AddComponent(emitter = new Sound3DEmitter())
            .AddComponent(particleSystem = new ParticleSystem3D()
            {
                NumParticles = 25,
                EmitRate = 300000,
                MinLife = TimeSpan.FromSeconds(1),
                MaxLife = TimeSpan.FromSeconds(1.5),
                LocalVelocity = Vector3.Zero,
                RandomVelocity = Vector3.One * 0.7f,
                MinSize = 20,
                MaxSize = 25,
                MaxRotateSpeed = 0.025f,
                MinRotateSpeed = -0.055f,
                InitialAngle = MathHelper.TwoPi,
                EndDeltaScale = 2f,
                EmitterSize = new Vector2(4),
                EmitterShape = ParticleSystem3D.Shape.FillRectangle,
                InterpolationColors = new List<Color>() { Color.White, Color.White, Color.Black },
                LinearColorEnabled = true,
                Gravity = Vector3.Zero,
                Emit = false
            })
            .AddComponent(new ParticleSystemRenderer3D())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Explosion/Explosion_1.png", DefaultLayers.Additive) { VertexColorEnabled = true }))
            ;

            this.particleSystems.Add(particleSystem);


            var emitter2 = new Entity()
            .AddComponent(new Transform3D())
            .AddComponent(particleSystem = new ParticleSystem3D()
            {
                NumParticles = 4,
                EmitRate = 300000,
                MinLife = TimeSpan.FromSeconds(0.2),
                MaxLife = TimeSpan.FromSeconds(0.3),
                LocalVelocity = Vector3.Zero,
                RandomVelocity = Vector3.One * 0.01f,
                MinSize = 30,
                MaxSize = 40,
                InitialAngle = MathHelper.TwoPi,
                MaxRotateSpeed = 0.1f,
                MinRotateSpeed = -0.1f,
                EndDeltaScale = 0.3f,
                EmitterSize = new Vector2(1),
                EmitterShape = ParticleSystem3D.Shape.FillRectangle,
                InterpolationColors = new List<Color>() { Color.White, Color.Black },
                LinearColorEnabled = true,
                Gravity = Vector3.Zero,
                Emit = false
            })
            .AddComponent(new ParticleSystemRenderer3D())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Explosion/Explosion_3.png", DefaultLayers.Additive) { VertexColorEnabled = true }))
            ;

            this.particleSystems.Add(particleSystem);

            var emitter3 = new Entity()
            .AddComponent(new Transform3D())
            .AddComponent(particleSystem = new ParticleSystem3D()
            {
                NumParticles = 60,
                EmitRate = 300000,
                MinLife = TimeSpan.FromSeconds(2),
                MaxLife = TimeSpan.FromSeconds(2.4),
                LocalVelocity = Vector3.Zero,
                RandomVelocity = Vector3.One * 1f,
                MinSize = 0.7f,
                MaxSize = 0.7f,
                InitialAngle = MathHelper.TwoPi,
                Gravity = Vector3.Zero,
                EndDeltaScale = 1,
                EmitterSize = new Vector2(1),
                EmitterShape = ParticleSystem3D.Shape.FillRectangle,
                InterpolationColors = new List<Color>() { Color.White, Color.Black },
                LinearColorEnabled = true,
                Emit = false
            })
            .AddComponent(new ParticleSystemRenderer3D())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Blaster.png", DefaultLayers.Additive) { VertexColorEnabled = true }))
            ;

            this.particleSystems.Add(particleSystem);

            var emitter4 = new Entity()
            .AddComponent(new Transform3D())
            .AddComponent(particleSystem = new ParticleSystem3D()
            {
                NumParticles = 15,
                EmitRate = 300000,
                MinLife = TimeSpan.FromSeconds(1.5),
                MaxLife = TimeSpan.FromSeconds(4),
                LocalVelocity = Vector3.Zero,
                RandomVelocity = Vector3.One * 0.2f,
                MinSize = 15,
                MaxSize = 20,
                MaxRotateSpeed = 0.015f,
                MinRotateSpeed = -0.015f,
                InitialAngle = MathHelper.TwoPi,
                EndDeltaScale = 2f,
                EmitterSize = new Vector2(4),
                EmitterShape = ParticleSystem3D.Shape.FillRectangle,
                AlphaEnabled = true,
                InterpolationColors = new List<Color>() { Color.White * 0.5f, Color.Black },

                LinearColorEnabled = true,

                Gravity = Vector3.Zero,
                Emit = false
            })
            .AddComponent(new ParticleSystemRenderer3D())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Explosion/Explosion_5.png", DefaultLayers.Alpha) { VertexColorEnabled = true }))
            ;

            this.particleSystems.Add(particleSystem);


            Entity shockwave = new Entity("shockwave")
            .AddComponent(new Transform3D() { Rotation = new Vector3(0.3f, -0.2f, -0.4f)})
            .AddComponent(new Model("Content/Models/Plane.FBX"))
            .AddComponent(new ModelRenderer())
            .AddComponent(this.shockwave = new ShockwaveBehavior())
            .AddComponent(new MaterialsMap(new BasicMaterial("Content/Textures/Explosion/Explosion_4.png", DefaultLayers.Additive)))
            ;


            this.entity.AddChild(emitter1);
            this.entity.AddChild(emitter2);
            this.entity.AddChild(emitter3);
            this.entity.AddChild(emitter4);
            this.entity.AddChild(shockwave);

            this.entity.EntityInitialized += (s, e) =>
                {
                    var screenplay = this.entity.Scene.EntityManager.Find("ScreenplayManager").FindComponent<ScreenplayManager>();
                    screenplay.FrameEvent(frame, this.BayExplosion);
                };
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

            this.emitter.Play(SoundType.Explosion, 3, false);
            this.shockwave.StartShockWave();

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

            this.entity.ChildEntities.First().FindComponent<ParticleSystem3D>().Emit = false;
        }
    }
}
