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

#region File Description
//-----------------------------------------------------------------------------
// MyScene
//
// Copyright © 2012 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

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

namespace RopeProject
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
                .AddComponent(Model.CreatePlane(Vector3.Up, 100))
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(ground);

            List<Entity> entities = new List<Entity>();
            int max = 15;
            for (int i = 0; i < 9; i++)
            {
                if (i == 0)
                {
                    entities.Add(CreateStaticBox("Box" + i, new Vector3(0, max, 0), new Vector3(1.2f), 1));
                }
                else
                {
                    entities.Add(CreateBox("Box" + i, new Vector3(0, (i * -1.5f) + max, 0), Vector3.One, 1));
                }
            }

            for (int i = 0; i < entities.Count; i++)
            {
                if (i > 0)
                {
                    entities[i].AddComponent(new BallSocketJoint(entities[i - 1], null));
                }

                EntityManager.Add(entities[i]);
            }
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

        private Entity CreateStaticBox(string name, Vector3 position, Vector3 scale, float mass)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position, Scale = scale })
                .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor())))
                .AddComponent(Model.CreateCube())
                .AddComponent(new BoxCollider())
                .AddComponent(new RigidBody3D() { Mass = mass, IsKinematic = true })
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
