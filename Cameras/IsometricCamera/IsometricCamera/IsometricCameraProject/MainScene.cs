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
using System.Linq;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Components;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Materials;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Components.Graphics3D;

namespace IsometricCameraProject
{
    public class MainScene : Scene
    {
        private int cubeIndex = 0;

        protected override void CreateScene()
        {
            Entity isometricCamera = new Entity("isometric")
                                    .AddComponent(new Camera3D()
                                    {
                                        Position = new Vector3(10f, 25f, 30f),
                                        LookAt = Vector3.Zero,
                                        BackgroundColor = Color.CornflowerBlue,
                                    })
                                    .AddComponent(new IsometricCameraBehavior());

            EntityManager.Add(isometricCamera);

            var cubes = 10;

            CreateCube(Vector3.Zero, new Vector3(20f, 1f, 1f));
            CreateCube(new Vector3(0f, -1.01f, 0f), new Vector3(25f, 1f, 25f));

            for (int i = 1; i < cubes; i++)
            {
                CreateCube(new Vector3(i * -1f, 0f, i * -1f));
                CreateCube(new Vector3(i * 1f, 0f, i * 1f));
            }
        }

        private void CreateCube(Vector3 position)
        {
            CreateCube("Cube_" + cubeIndex, position, Vector3.One);
        }

        private void CreateCube(Vector3 position, Vector3 scale)
        {
            CreateCube("Cube_" + cubeIndex, position, scale);
        }

        private void CreateCube(string cubeName, Vector3 position, Vector3 scale)
        {
            Color color = GetRandomColor();

            var cube = new Entity(cubeName)
                                  .AddComponent(new Transform3D() { Position = position, Scale = scale })
                                  .AddComponent(Model.CreateCube())
                                  .AddComponent(new MaterialsMap(new BasicMaterial(color)))
                                  .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);

            cubeIndex++;
        }

        private static Color GetRandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
