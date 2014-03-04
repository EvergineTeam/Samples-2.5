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

namespace FrictionProject
{
    public class MyScene : Scene
    {
        Entity cube1;
        Entity cube2;
        Entity cube3;
        Entity cube4;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(-5, 8, 22), Vector3.Zero);
            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            Entity ground = new Entity("Ground")
                .AddComponent(new Transform3D() { Position = new Vector3(0, 0, 0), Scale = new Vector3(20, 1, 20) })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Beige) ))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(ground);

            cube1 = CreateCube("Cube1", new Vector3(9, 1, 9), 0.04f);
            cube2 = CreateCube("Cube2", new Vector3(9, 1, 5), .1f);
            cube3 = CreateCube("Cube3", new Vector3(9, 1, -5), .2f);
            cube4 = CreateCube("Cube4", new Vector3(9, 1, -9), .4f);

            EntityManager.Add(cube1);
            EntityManager.Add(cube2);
            EntityManager.Add(cube3);
            EntityManager.Add(cube4);
        }

        protected override void Start()
        {
            base.Start();

            Vector3 impulse = new Vector3(-5, 0, 0);

            cube1.FindComponent<RigidBody3D>().ApplyLinearImpulse(impulse);
            cube2.FindComponent<RigidBody3D>().ApplyLinearImpulse(impulse);
            cube3.FindComponent<RigidBody3D>().ApplyLinearImpulse(impulse);
            cube4.FindComponent<RigidBody3D>().ApplyLinearImpulse(impulse);
        }

        private Entity CreateCube(string name, Vector3 position, float friction)
        {
            Entity primitive = new Entity(name)
                .AddComponent(new Transform3D() { Position = position })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { KineticFriction = friction, StaticFriction = friction })
                .AddComponent(new MaterialsMap(new BasicMaterial(GetRandomColor()) ))
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
