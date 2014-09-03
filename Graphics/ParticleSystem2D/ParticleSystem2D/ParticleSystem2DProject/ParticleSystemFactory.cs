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
        public static ParticleSystem2D CreateSmokeParticle()
        {
            ParticleSystem2D pSystem = new ParticleSystem2D()
            {
                SortEnabled = true,
                NumParticles = 150,
                EmitRate = 50,
                MinColor = Color.White,
                MaxColor = Color.White,
                MinLife = TimeSpan.FromSeconds(2),
                MaxLife = TimeSpan.FromSeconds(4),
                LocalVelocity = new Vector2(0.2f, 0f),
                RandomVelocity = new Vector2(0.1f, 0.1f),
                MinSize = 8,
                MaxSize = 15,
                EndDeltaScale = 4f,
                MinRotateSpeed = -0.01f,
                MaxRotateSpeed = 0.01f,
                EmitterSize = new Vector3(10),
                EmitterShape = ParticleSystem2D.Shape.FillCircle,
                InterpolationColors = new List<Color>() { Color.White, Color.Transparent },
                LinearColorEnabled = true,
                AlphaEnabled = true,

            };

            return pSystem;
        }

        public static ParticleSystem2D CreateFireParticle()
        {
            ParticleSystem2D fireParticle = new ParticleSystem2D()
            {
                NumParticles = 80,
                EmitRate = 130,
                MinLife = TimeSpan.FromSeconds(0.2f),
                MaxLife = TimeSpan.FromSeconds(1f),
                LocalVelocity = new Vector2(0.2f, -0.2f),
                RandomVelocity = new Vector2(0.4f, 0.4f),
                MinSize = 15,
                MaxSize = 30,
                MinRotateSpeed = 0.1f,
                MaxRotateSpeed = -0.1f,
                EndDeltaScale = 0.0f,
                EmitterSize = new Vector3(10),
                EmitterShape = ParticleSystem2D.Shape.FillCircle
            };

            return fireParticle;
        }

        public static ParticleSystem2D CreateExplosion()
        {
            ParticleSystem2D explosionParticle = new ParticleSystem2D()
            {
                NumParticles = 200,
                EmitRate = 1500,
                MinLife = TimeSpan.FromSeconds(1f),
                MaxLife = TimeSpan.FromSeconds(3f),
                LocalVelocity = new Vector2(0.4f, -2f),
                RandomVelocity = new Vector2(2f, 1.5f),
                MinSize = 15,
                MaxSize = 40,
                MinRotateSpeed = 0.03f,
                MaxRotateSpeed = -0.03f,
                EndDeltaScale = 0f,
                EmitterSize = new Vector3(30),
                Gravity = new Vector2(0, 0.03f),
                EmitterShape = ParticleSystem2D.Shape.FillCircle,
            };

            return explosionParticle;
        }

        public static ParticleSystem2D CreateDinosaurs()
        {
            ParticleSystem2D dinoParticle = new ParticleSystem2D()
            {
                NumParticles = 10,
                EmitRate = 60,
                MinColor = Color.Black,
                MaxColor = Color.Black,
                MinLife = TimeSpan.FromSeconds(2f),
                MaxLife = TimeSpan.FromSeconds(4f),
                LocalVelocity = new Vector2(0.4f, -5f),
                RandomVelocity = new Vector2(3f, 1f),
                MinSize = 10,
                MaxSize = 25,
                MinRotateSpeed = 0.06f,
                MaxRotateSpeed = -0.06f,
                InitialAngleVariation = 100,
                EmitterSize = new Vector3(30),
                Gravity = new Vector2(0, 0.08f),
                EmitterShape = ParticleSystem2D.Shape.FillCircle,
                InterpolationColors = new List<Color>() { Color.White, Color.Transparent },
            };

            return dinoParticle;
        }
    }
}
