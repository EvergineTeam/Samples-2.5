using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Particles;

namespace ParticleSystem2DProject
{
    public static class ParticleSystemFactory
    {
        public static WaveEngine.Components.Particles.ParticleSystem2D CreateSmokeParticle()
        {
            WaveEngine.Components.Particles.ParticleSystem2D pSystem = new WaveEngine.Components.Particles.ParticleSystem2D()
            {
                SortEnabled = true,
                NumParticles = 150,
                EmitRate = 50,
                MinColor = Color.White,
                MaxColor = Color.White,
                MinLife = 2,
                MaxLife = 4,
                LocalVelocity = new Vector2(0.2f, 0f),
                RandomVelocity = new Vector2(0.1f, 0.1f),
                MinSize = 8,
                MaxSize = 15,
                EndDeltaScale = 4f,
                MinRotateSpeed = -0.01f,
                MaxRotateSpeed = 0.01f,
                EmitterSize = new Vector3(10),
                EmitterShape = WaveEngine.Components.Particles.ParticleSystem2D.Shape.FillCircle,
                InterpolationColors = new List<Color>() { Color.White, Color.Transparent },
                LinearColorEnabled = true,
                AlphaEnabled = true,

            };

            return pSystem;
        }

        public static WaveEngine.Components.Particles.ParticleSystem2D CreateFireParticle()
        {
            WaveEngine.Components.Particles.ParticleSystem2D fireParticle = new WaveEngine.Components.Particles.ParticleSystem2D()
            {
                NumParticles = 80,
                EmitRate = 130,
                MinLife = 0.2f,
                MaxLife = 1f,
                LocalVelocity = new Vector2(0.2f, -0.2f),
                RandomVelocity = new Vector2(0.4f, 0.4f),
                MinSize = 15,
                MaxSize = 30,
                MinRotateSpeed = 0.1f,
                MaxRotateSpeed = -0.1f,
                EndDeltaScale = 0.0f,
                EmitterSize = new Vector3(10),
                EmitterShape = WaveEngine.Components.Particles.ParticleSystem2D.Shape.FillCircle,
            };

            return fireParticle;
        }

        public static WaveEngine.Components.Particles.ParticleSystem2D CreateExplosion()
        {
            WaveEngine.Components.Particles.ParticleSystem2D explosionParticle = new WaveEngine.Components.Particles.ParticleSystem2D()
            {
                NumParticles = 200,
                EmitRate = 1500,
                MinLife = 1f,
                MaxLife = 3f,
                LocalVelocity = new Vector2(0.4f, -2f),
                RandomVelocity = new Vector2(2f, 1.5f),
                MinSize = 15,
                MaxSize = 40,
                MinRotateSpeed = 0.03f,
                MaxRotateSpeed = -0.03f,
                EndDeltaScale = 0f,
                EmitterSize = new Vector3(30),
                Gravity = new Vector2(0, 0.03f),
                EmitterShape = WaveEngine.Components.Particles.ParticleSystem2D.Shape.FillCircle,
            };

            return explosionParticle;
        }

        public static WaveEngine.Components.Particles.ParticleSystem2D CreateDinosaurs()
        {
            WaveEngine.Components.Particles.ParticleSystem2D dinoParticle = new WaveEngine.Components.Particles.ParticleSystem2D()
            {
                NumParticles = 10,
                EmitRate = 60,
                MinColor = Color.Black,
                MaxColor = Color.Black,
                MinLife = 2f,
                MaxLife = 4f,
                LocalVelocity = new Vector2(0.4f, -5f),
                RandomVelocity = new Vector2(3f, 1f),
                MinSize = 10,
                MaxSize = 25,
                MinRotateSpeed = 0.06f,
                MaxRotateSpeed = -0.06f,
                InitialAngleVariation = 100,
                EmitterSize = new Vector3(30),
                Gravity = new Vector2(0, 0.08f),
                EmitterShape = WaveEngine.Components.Particles.ParticleSystem2D.Shape.FillCircle,
                InterpolationColors = new List<Color>() { Color.White, Color.Transparent },
            };

            return dinoParticle;
        }
    }
}
