// Copyright (C) 2014 Weekend Game Studio
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
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace HingleJointProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 10, 20), new Vector3(0, 5, 0))
            {
                BackgroundColor = Color.CornflowerBlue,
            };
            camera.Entity.AddComponent(new FireBehavior());

            EntityManager.Add(camera.Entity);

            Entity ground = new Entity("Ground")
                .AddComponent(new Transform3D() { Position = new Vector3(0, -1, 0), Scale = new Vector3(100, 1, 100) })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
                .AddComponent(new ModelRenderer());

            EntityManager.Add(ground);

            Entity a = CreateStaticBox("Box1", new Vector3(0, 4.1f, 0), new Vector3(0.2f, 8, 0.2f), 1);
            Entity b = CreateBox("Box2", new Vector3(2.7f, 4.1f, 0), new Vector3(5, 8, 0.2f), 4);

            b.AddComponent(new JointMap3D()
                                    .AddJoint("hingeJoint", new HingeJoint(a, a.FindComponent<Transform3D>().Position, Vector3.Up)));

            EntityManager.Add(a);
            EntityManager.Add(b);
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
            return new Color(WaveServices.Random.NextUInt());
        }
    }
}
