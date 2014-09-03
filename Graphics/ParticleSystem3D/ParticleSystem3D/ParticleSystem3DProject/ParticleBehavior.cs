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
using System.Linq;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Components.UI;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Particles;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
using System.Collections.Generic;
using WaveEngine.Common.Math;

namespace ParticleSystem3DProject
{
    public class ParticleBehavior : Behavior
    {
        private TextBlock textBlock;

        private int currentParticleSystemIndex;

        private Input input;

        private MouseState lastState;


        public ParticleBehavior() :
            base("ParticleBehavior")
        {
            currentParticleSystemIndex = 0;
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            
            // TODO: Pending of EntityManager.Find return decorator classes
            this.textBlock = (Owner.Scene as MyScene).textBlock1;
        }

        protected override void Update(TimeSpan gameTime)
        {
            input = WaveServices.Input;

            if (input.MouseState.IsConnected)
            {
                if (input.MouseState.RightButton == ButtonState.Pressed && lastState.RightButton == ButtonState.Release)
                {

                    currentParticleSystemIndex++;
                    currentParticleSystemIndex = currentParticleSystemIndex % 6;

                    ApplyChanges();
                    ApplyMaterial();

                    textBlock.Text = GetParticleName();
                    Owner.RefreshDependencies();
                }
                lastState = input.MouseState;
            }
        }

        public void ApplyChanges()
        {
            var currentParticle = ParticleEntityFactory.CreateParticleSystem3D(currentParticleSystemIndex);

            Owner.RemoveComponent<ParticleSystem3D>();

            Owner.AddComponent(currentParticle);

            switch (currentParticleSystemIndex)
            {
                case 2:
                    {
                        var transform = Owner.FindComponent<Transform3D>();
                        transform.Position = new Vector3(0f, 1000f, 0f);
                        transform.Scale = new Vector3(0.01f, 2f, 1f);
                        break;
                    }
                case 3:
                    {
                        var transform = Owner.FindComponent<Transform3D>();
                        transform.Position = new Vector3(0f, 1000f, 0f);
                        transform.Scale = Vector3.One;
                        break;
                    }
                default:
                    {
                        var transform = Owner.FindComponent<Transform3D>();
                        transform.Position = Vector3.Zero;
                        transform.Scale = Vector3.One;
                        break;
                    }
            }
        }

        public void ApplyMaterial()
        {
            Owner.RemoveComponent<MaterialsMap>();

            string content = "Content/SmokeParticle.wpk";

            if (currentParticleSystemIndex == 2
                || currentParticleSystemIndex == 3)
            {
                content = "Content/WhitePixel.wpk";
            }
            var materials = new MaterialsMap(new BasicMaterial(content, DefaultLayers.Additive) { VertexColorEnabled = true });

            Owner.AddComponent(materials);
        }

        private string GetParticleName()
        {
            switch (currentParticleSystemIndex)
            {
                case 0:
                    {
                        return "Fire";
                    }
                case 1:
                    {
                        return "Fog";
                    }
                case 2:
                    {
                        return "Rain";
                    }
                case 3:
                    {
                        return "Snow";
                    }
                case 4:
                    {
                        return "Sparks";
                    }
                case 5:
                    {
                        return "Vapor";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}
