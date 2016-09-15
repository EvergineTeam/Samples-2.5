#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace PhysicExplosion.Behaviors
{
    [DataContract]
    public class ExplosionBehavior : Behavior
    {
        private List<Entity> particles;
        private List<Entity> removeParticles;
        private bool explosion;
        private int numParticles;
        private float density;
        private SpriteRenderer render;

        #region Properties

        /// <summary>
        /// Gets or sets number of particles
        /// </summary>
        [DataMember]
        public int NumParticles
        {
            get { return this.numParticles; }
            set
            {
                this.numParticles = value;
                this.density = 60.0f / value;
            }
        }

        /// <summary>
        /// Gets or sets blast power
        /// </summary>
        [DataMember]
        public int BlastPower { get; set; }

        /// <summary>
        /// Gets or sets delay
        /// </summary>
        [DataMember]
        public TimeSpan Delay { get; set; }
        #endregion

        /// <summary>
        /// Default values method
        /// </summary>
        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.NumParticles = 1;
            this.BlastPower = 100;
            this.density = 60.0f / NumParticles;
            this.Delay = TimeSpan.FromSeconds(2.0f);
            this.explosion = false;
        }

        /// <summary>
        /// Initialize method
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.particles = new List<Entity>();
            this.removeParticles = new List<Entity>();
        }

        /// <summary>
        /// Resolve dependencies method
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.render = this.Owner.FindComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Create particles method
        /// </summary>
        private void CreateParticles()
        {
            float randomStartAngle = (float)WaveServices.Random.NextDouble() * 90;

            for (int i = 0; i < NumParticles; i++)
            {
                float angle = MathHelper.ToRadians(((i / (float)NumParticles) * 360) + randomStartAngle);
                Entity particle = this.CreateParticle(angle, this.density, this.BlastPower);
                this.particles.Add(particle);
                this.Owner.AddChild(particle);
            }
        }

        /// <summary>
        /// Create particle method
        /// </summary>
        /// <param name="angle">initial angle</param>
        /// <param name="density">particle density</param>
        /// <param name="blastPower">blast power</param>
        /// <returns>Particle entity</returns>
        private Entity CreateParticle(float angle, float density, float blastPower)
        {
            Vector2 direction = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));

            return new Entity() { Tag = "particle" }
                            .AddComponent(new Transform2D()
                            {
                                Rectangle = new RectangleF(0, 0, 10, 10),
                            })
                            .AddComponent(new CircleCollider2D()
                            {
                                Radius = 1,
                                Density = density,
                                Friction = 0.0f,
                                Restitution = 0.99f,
                                GroupIndex = -10,
                                CollidesWith = ColliderCategory2D.Cat1,
                            })
                            .AddComponent(new RigidBody2D()
                            {
                                PhysicBodyType = RigidBodyType2D.Dynamic,
                                FixedRotation = true,
                                IsBullet = true,
                                LinearDamping = 7,
                                GravityScale = 0,
                                LinearVelocity = direction * blastPower,
                            })
                            .AddComponent(new ExplosionParticleBehavior());
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime">game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            if (!this.explosion)
            {
                if (this.Delay > TimeSpan.Zero)
                {
                    this.Delay -= gameTime;
                }
                else
                {
                    this.CreateParticles();
                    this.explosion = true;
                    this.render.IsVisible = false;
                }
            }
            else
            {
                if (this.particles.Count <= 0)
                {
                    this.EntityManager.Remove(this.Owner);
                    return;
                }

                // Update physic particles
                this.removeParticles.Clear();
                foreach (Entity particle in this.particles)
                {
                    if (particle.FindComponent<ExplosionParticleBehavior>().Killed)
                    {
                        this.removeParticles.Add(particle);
                    }
                }

                foreach (Entity particle in this.removeParticles)
                {
                    this.Owner.RemoveChild(particle.Name);
                    this.particles.Remove(particle);
                }
            }
        }
    }
}
