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
using WaveEngine.Components.Graphics3D;

namespace OrthogonalCameraProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            Entity orthogonalCamera = new Entity("orthogonal")
                                        .AddComponent(new Camera()
                                        {
                                            Position = new Vector3(0,15f,15f),
                                            LookAt = Vector3.Zero,
                                        })
                                        .AddComponent(new CameraBehavior());

            RenderManager.SetActiveCamera(orthogonalCamera);
            EntityManager.Add(orthogonalCamera);

            Color color1 = Color.LightSalmon;
            Color color2 = Color.LightGreen;

            var offset = 2f;

            for (int i = 0; i < 5; i++)
            {
                CreateCube("CubeTop" + i, new Vector3(i * offset, (1 + i) * -offset, i * offset), color1);

                CreateCube("CubeBottom" + i, new Vector3(i * offset, (1 + i) * offset, i * offset), color1);

                CreateCube("CubeLeft" + i, new Vector3(i * -offset, (1 + i) * -offset, i * offset), color2);

                CreateCube("CubeRight" + i, new Vector3(i * -offset, (1 + i) * offset, i * offset), color2);

                CreateCube("CubeBaseTop" + i, new Vector3(0, (1 + i) * offset, 0), color1);

                CreateCube("CubeBaseBottom" + i, new Vector3(0, (1 + i) * -offset, 0), color2);

                CreateCube("CubeBaseLeft" + i, new Vector3(i * -offset, 0f, 0f), color2);

                CreateCube("CubeBaseRight" + i, new Vector3(i * offset, 0f, 0f), color2);

                CreateCube("CubeBackTop" + i, new Vector3(i * offset, (1 + i) * -offset, i * -offset), color1);

                CreateCube("CubeBackBottom" + i, new Vector3(i * offset, (1 + i) * offset, i * -offset), color2);

                CreateCube("CubeBackLeft" + i, new Vector3(i * -offset, (1 + i) * -offset, i * -offset), color1);

                CreateCube("CubeBackRight" + i, new Vector3(i * -offset, (1 + i) * offset, (i * -offset)), color2);
            }

            RenderManager.BackgroundColor = Color.CornflowerBlue;
        }

        private void CreateCube(string name, Vector3 position, Color color)
        {
            var cube = new Entity(name)
                        .AddComponent(new Transform3D() { Position = position })
                        .AddComponent(Model.CreateCube())
                        .AddComponent(new MaterialsMap(new BasicMaterial(color)))
                        .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);
        }
    }
}
