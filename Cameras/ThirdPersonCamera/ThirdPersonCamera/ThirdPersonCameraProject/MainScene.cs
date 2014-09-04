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

using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Framework;
using WaveEngine.Materials;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Components.Cameras;

namespace ThirdPersonCameraProject
{
    public class MainScene : Scene
    {
        protected override void CreateScene()
        {
            var player = CreatePlayer();
            ThirdPersonCamera thirdPersonCamera = new ThirdPersonCamera("thirdPerson", player);
            thirdPersonCamera.BackgroundColor = Color.CornflowerBlue;

            EntityManager.Add(thirdPersonCamera);

            CreateCube("Cube2", new Vector3(5f, 0f, 0f));
            CreateCube("Cube3", new Vector3(-5f, 0f, 0f));
            CreateCube("Cube4", new Vector3(5f, 0f, 5f));
            CreateCube("Cube5", new Vector3(-5f, 0f, 5f));
            CreateCube("Cube6", new Vector3(5f, 0f, -5f));
            CreateCube("Cube7", new Vector3(-5f, 0f, -5f));
        }

        private void CreateCube(string cubeName, Vector3 position)
        {
            var cube = new Entity(cubeName)
                                  .AddComponent(new Transform3D() { Position = position })
                                  .AddComponent(Model.CreateCube())
                                  .AddComponent(new MaterialsMap(new BasicMaterial(Color.Red)))
                                  .AddComponent(new ModelRenderer());

            EntityManager.Add(cube);
        }

        private Entity CreatePlayer()
        {
            var player = new Entity("Player")
                                  .AddComponent(new Transform3D())
                                  .AddComponent(Model.CreateCube())
                                  .AddComponent(new MaterialsMap(new BasicMaterial(Color.Yellow)))
                                  .AddComponent(new ModelRenderer())
                                  .AddComponent(new PlayerBehavior());
            EntityManager.Add(player);

            return player;
        }
    }
}
