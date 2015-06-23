#region File Description
//-----------------------------------------------------------------------------
// ExplosionEmitter
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using WaveEngine.Framework;
using System;
using Kinect2DGameSampleProject.Behaviors;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;

namespace Kinect2DGameSampleProject.Entities
{
    /// <summary>
    /// Explosion Emitter
    /// </summary>
    public class ExplosionEmitter : BaseDecorator
    {
        /// <summary>
        /// The standposition
        /// </summary>
        protected Vector2 standPosition = new Vector2(0, -100);

        /// <summary>
        /// Gets the transform 2D.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        public Transform2D Transform
        {
            get
            {
                return this.entity.FindComponent<Transform2D>();
            }
        }

        /// <summary>
        /// Gets the explosion behavior.
        /// </summary>
        /// <value>
        /// The explosion behavior.
        /// </value>
        public ExplosionBehavior ExplosionBehavior
        {
            get
            {
                return this.entity.FindComponent<ExplosionBehavior>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionEmitter" /> class.
        /// </summary>
        public ExplosionEmitter()
        {
            this.entity = new Entity()
                .AddComponent(new Transform2D() { Origin = Vector2.Center, Position = this.standPosition, DrawOrder = -1 })
                .AddComponent(ParticleSystemFactory.CreateExplosionParticle())
                .AddComponent(new ExplosionBehavior() { Duration = TimeSpan.FromSeconds(0.1f) })
                .AddComponent(new Material2D(new BasicMaterial2D("Content/Smoke.wpk", DefaultLayers.Alpha)))
                .AddComponent(new ParticleSystemRenderer2D("particleRenderer"));
        }
    }
}
