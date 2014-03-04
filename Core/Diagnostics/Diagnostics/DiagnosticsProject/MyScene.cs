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
using WaveEngine.Components;
using WaveEngine.Components.Cameras;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Framework.Graphics;
using WaveEngine.Components.Graphics3D;

namespace DiagnosticsProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Black;
            // Set to true the diagnostic value
            WaveServices.ScreenContextManager.SetDiagnosticsActive(true);
            // And set the compilation symbol PROFILE in the project

            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(-100f, -100f, -100f), Vector3.Zero)
            {
                Speed = 500f,
            };    
            EntityManager.Add(mainCamera.Entity);

            RenderManager.SetActiveCamera(mainCamera.Entity);

            float radius = 15;
            float angleStep = 1 / radius;
            var scale = 5f;
            for (double angle = 0; angle < Math.PI * 2; angle += angleStep * 2)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                CreateCube("Cube1_" + angle, new Vector3((x * scale) + scale, 0, (y * scale) + scale), scale, (float)angle, 1f);
                CreateCube("Cube2_" + angle, new Vector3(0f, (x * scale) + scale, (y * scale) + scale), scale, (float)angle, 0.5f);
                CreateCube("Cube3_" + angle, new Vector3((x * scale) + scale, (y * scale) + scale, 0f), scale, (float)angle, 0.75f);
            }

            RenderManager.BackgroundColor = Color.CornflowerBlue;
        }

        private void CreateCube(string name, Vector3 position, float scaleFactor, float angleStep, float speed)
        {
            var cube = new Entity(name)
                           .AddComponent(new Transform3D() { Position = position, Scale = new Vector3(scaleFactor) })
                           .AddComponent(Model.CreateCube())
                            .AddComponent(new MaterialsMap(new BasicMaterial(GenerateRandomColors())))
                       .AddComponent(new ModelRenderer())
                       .AddComponent(new CubeBehavior(name, angleStep, speed));

            EntityManager.Add(cube);
        }

        private Color GenerateRandomColors()
        {
            var random = WaveServices.Random;

            var color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 255);

            return color;
        }
    }
}
