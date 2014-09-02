// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Particles;
using WaveEngine.Framework;
using WaveEngine.Materials;
using WaveEngine.Framework.Graphics;
using WaveEngine.Components.Graphics3D;

namespace ParticleSystem3DProject
{
    /// <summary>
    /// A custom factory to create some differents particle systems
    /// </summary>
    public static class ParticleEntityFactory
    {
        /// <summary>
        /// A fire particle system
        /// </summary>
        /// <returns></returns>
        public static ParticleSystem3D CreateFireParticleSystem3D()
        {
            ParticleSystem3D pSystem = new ParticleSystem3D()
            {
                NumParticles = 500,
                MinColor = Color.Black,
                MaxColor = Color.Black,
                MinLife = TimeSpan.FromSeconds(3),
                MaxLife = TimeSpan.FromSeconds(5),
                LocalVelocity = new Vector3(0.0f, 3.0f, 0.0f),
                RandomVelocity = new Vector3(0.2f, 1.0f, 0.2f),
                MinSize = 40,
                MaxSize = 70,
                EndDeltaScale = 3f,
                EmitterSize = new Vector2(500),
                EmitterShape = ParticleSystem3D.Shape.Circle,
                InterpolationColors = new List<Color>() { Color.Red, Color.Orange },
                LinearColorEnabled = true,
                AlphaEnabled = true,
            };
            return pSystem;
        }

        /// <summary>
        /// A fog particle system
        /// </summary>
        /// <returns></returns>
        public static ParticleSystem3D CreateFogParticleSystem3D()
        {
            ParticleSystem3D pSystem = new ParticleSystem3D()
           {
               NumParticles = 100,
               MinColor = Color.Black,
               MaxColor = Color.Black,
               MinLife = TimeSpan.FromSeconds(3),
               MaxLife = TimeSpan.FromSeconds(4),
               LocalVelocity = new Vector3(0.0f, 3.0f, 0.0f),
               RandomVelocity = new Vector3(0.2f, 1.0f, 0.2f),
               MinSize = 40,
               MaxSize = 70,
               EndDeltaScale = 3f,
               EmitterSize = new Vector2(500),
               EmitterShape = ParticleSystem3D.Shape.FillCircle,
               InterpolationColors = new List<Color>() { new Color(226, 207, 190), new Color(200, 153, 109), Color.Transparent },
               LinearColorEnabled = true,
               AlphaEnabled = true,
           };
            return pSystem;
        }

        /// <summary>
        /// A vapor particle system
        /// </summary>
        /// <returns></returns>
        public static ParticleSystem3D CreateVaporParticleSystem3D()
        {

            ParticleSystem3D pSystem = new ParticleSystem3D()
            {
                NumParticles = 100,
                MinColor = Color.Black,
                MaxColor = Color.Black,
                MinLife = TimeSpan.FromSeconds(2),
                MaxLife = TimeSpan.FromSeconds(4),
                LocalVelocity = new Vector3(0.0f, 3.0f, 0.0f),
                RandomVelocity = new Vector3(10f, 10f, 10f),
                MinSize = 40,
                MaxSize = 50,
                EndDeltaScale = 10f,
                EmitterSize = new Vector2(100),
                EmitterShape = ParticleSystem3D.Shape.Rectangle,
                InterpolationColors = new List<Color>() { Color.White },
                LinearColorEnabled = true,
                AlphaEnabled = true,
            };
            return pSystem;
        }

        /// <summary>
        /// A snow particle system
        /// </summary>
        /// <returns></returns>
        public static ParticleSystem3D CreateSnowParticleSystem3D()
        {
            ParticleSystem3D pSystem = new ParticleSystem3D()
          {
              NumParticles = 700,
              MinColor = Color.White,
              MaxColor = Color.Gray,
              LocalVelocity = new Vector3(0f, 0.1f, 0f),
              RandomVelocity = new Vector3(1f, 0.2f, 1f),
              MinLife = TimeSpan.FromSeconds(3),
              MaxLife = TimeSpan.FromSeconds(6),
              InitialAngle = 0.5f,
              MinRotateSpeed = 0.01f,
              MaxRotateSpeed = 0.05f,
              MinSize = 1,
              MaxSize = 4,
              EmitterShape = ParticleSystem3D.Shape.FillRectangle,
              EmitterSize = new Vector2(1200, 200),
              Gravity = new Vector3(0f, -0.1f, 0f),
              InterpolationColors = new List<Color>() { Color.White, Color.Gray },
              LinearColorEnabled = true,
              AlphaEnabled = false,
              CollisionBehavior = ParticleSystem3D.ParticleCollisionBehavior.Bounce,
              CollisionType = ParticleSystem3D.ParticleCollisionFlags.MinY,
              CollisionMinY = 0,
              Bounciness = 0.0f
          };
            return pSystem;
        }

        /// <summary>
        /// A rain particle system
        /// </summary>
        /// <returns></returns>
        public static ParticleSystem3D CreateRainParticleSystem3D()
        {
            ParticleSystem3D pSystem = new ParticleSystem3D()
          {
              NumParticles = 2000,
              MinColor = Color.White,
              MaxColor = Color.Gray,
              LocalVelocity = new Vector3(0f, 3f, 0f),
              RandomVelocity = new Vector3(1f, 1f, 1f),
              MinLife = TimeSpan.FromSeconds(1.5f),
              MaxLife = TimeSpan.FromSeconds(3),
              InitialAngle = 0.5f,
              MinRotateSpeed = 0.01f,
              MaxRotateSpeed = 0.05f,
              MinSize = 1,
              MaxSize = 3,
              EmitterSize = new Vector2(1200, 200),
              EmitterShape = ParticleSystem3D.Shape.FillRectangle,
              Gravity = new Vector3(0, -1f, 0f),
              InterpolationColors = new List<Color>() { Color.White, Color.Gray },
              LinearColorEnabled = true,
              AlphaEnabled = false,
              CollisionBehavior = ParticleSystem3D.ParticleCollisionBehavior.Bounce,
              CollisionType = ParticleSystem3D.ParticleCollisionFlags.MinY,
              CollisionMinY = 0,
              CollisionSpread = new Vector3(15),
              Bounciness = 0.4f
          };
            return pSystem;
        }

        /// <summary>
        /// A sparks particle system
        /// </summary>
        /// <returns></returns>
        public static ParticleSystem3D CreateSparksParticleSystem3D()
        {
            ParticleSystem3D pSystem = new ParticleSystem3D()
            {
                NumParticles = 800,
                MinColor = Color.White,
                MaxColor = Color.Yellow,
                LocalVelocity = new Vector3(10f, 10f, 10f),
                RandomVelocity = new Vector3(5f, 10f, 5f),
                MinLife = TimeSpan.FromSeconds(3),
                MaxLife = TimeSpan.FromSeconds(9),
                InitialAngle = 0.5f,
                MinRotateSpeed = 0.01f,
                MaxRotateSpeed = 0.05f,
                MinSize = 20,
                MaxSize = 80,
                EmitterSize = new Vector2(50, 0),
                Gravity = new Vector3(0f, -0.3f, 0f),
                InterpolationColors = new List<Color>() { Color.White, Color.Gray },
                LinearColorEnabled = true,
                AlphaEnabled = false,
            };
            return pSystem;
        }

        public static ParticleSystem3D CreateParticleSystem3D(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        return CreateFireParticleSystem3D();
                    }
                case 1:
                    {
                        return CreateFogParticleSystem3D();
                    }
                case 2:
                    {
                        return CreateRainParticleSystem3D();
                    }
                case 3:
                    {
                        return CreateSnowParticleSystem3D();
                    }
                case 4:
                    {
                        return CreateSparksParticleSystem3D();
                    }
                case 5:
                    {
                        return CreateVaporParticleSystem3D();
                    }
                default:
                    {
                        return null;
                    }
            }

        }
    }
}
