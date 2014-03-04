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

#region Using Statements
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.Cameras;
#endregion

namespace WallDemoProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;
            //WaveServices.GraphicsDevice.RenderState.FillMode = FillMode.Wireframe;
            //RenderManager.DebugLines = true;

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 10, 20), new Vector3(0, 5, 0));
            camera.Entity.AddComponent(new FireBehavior());
           
            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            Entity ground = new Entity("Ground")
                .AddComponent(new Transform3D() { Position = new Vector3(0, -1, 0)})
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreatePlane(Vector3.Up, 50))
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(ground);

            #region CreateWall
            int width = 10;
            int height = 10;
            float blockWidth = 2f;
            float blockHeight = 1f;
            float blockLength = 1f;

            int n = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    n++;
                    var toAdd = CreateBox("box" + n, new Vector3(i * blockWidth + .5f * blockWidth * (j % 2) - width * blockWidth * .5f,
                                                                 blockHeight * .5f + j * (blockHeight),
                                                                 0),
                                                    new Vector3(blockWidth, blockHeight, blockLength), 10);

                    EntityManager.Add(toAdd);
                }
            }
            #endregion
        }

        private Entity CreateBox(string name, Vector3 position, Vector3 scale, float mass)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position, Scale = scale })
                .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor())))
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() { Mass = mass })
                .AddComponent(new ModelRenderer());

            return primitive;
        }

        private Color GetRandomColor()
        {
            var random = WaveServices.Random;
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f);
        }
    }
}
