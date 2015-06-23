#region File Description
//-----------------------------------------------------------------------------
// ParticleSystemFactory
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

using System;
using WaveEngine.Common.Math;
using WaveEngine.Components.Particles;

namespace Kinect2DGameSampleProject.Entities
{
    /// <summary>
    /// Particle System Factory
    /// </summary>
    public static class ParticleSystemFactory
    {
        /// <summary>
        /// Creates the fire particle.
        /// </summary>
        /// <returns>
        /// The Fire configured Particle System
        /// </returns>
        public static ParticleSystem2D CreateFireParticle()
        {
            ParticleSystem2D fireParticle = new ParticleSystem2D()
            {
                NumParticles = 40,
                EmitRate = 80,
                Gravity = new Vector2(0, -0.3f),
                MinLife = TimeSpan.FromSeconds(0.2f),
                MaxLife = TimeSpan.FromSeconds(1f),
                LocalVelocity = new Vector2(0.2f, -0.2f) * 3,
                RandomVelocity = new Vector2(0.4f, 0.4f) * 3,
                MinSize = 55,
                MaxSize = 100,
                MinRotateSpeed = 0.1f,
                MaxRotateSpeed = -0.1f,
                EndDeltaScale = 0.0f,
                EmitterSize = new Vector3(8),
                EmitterShape = ParticleSystem2D.Shape.FillCircle
            };

            return fireParticle;
        }

        /// <summary>
        /// Creates the explosion particle.
        /// </summary>
        /// <returns>
        /// The explosion Cconfigured Particle System
        /// </returns>
        public static ParticleSystem2D CreateExplosionParticle()
        {
            ParticleSystem2D fireParticle = new ParticleSystem2D()
            {
                NumParticles = 20,
                EmitRate = 20,
                Gravity = new Vector2(0, -0.05f),
                MinLife = TimeSpan.FromSeconds(0.8f),
                MaxLife = TimeSpan.FromSeconds(2f),
                LocalVelocity = new Vector2(0.1f, -0.1f) * 2,
                RandomVelocity = new Vector2(0.2f, 0.2f) * 2,
                MinSize = 80,
                MaxSize = 120,
                MinRotateSpeed = 0.05f,
                MaxRotateSpeed = -0.05f,
                EndDeltaScale = 0.0f,
                EmitterSize = new Vector3(25),
                EmitterShape = ParticleSystem2D.Shape.FillCircle
            };

            return fireParticle;
        }
    }
}
