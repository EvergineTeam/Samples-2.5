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
// MainScene
//
// Copyright © 2013 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace Entity3DCollisionGroupsProject
{
    /// <summary>
    /// Main Scene Class
    /// </summary>
    public class MainScene : Scene
    {
        /// <summary>
        /// Entities Instance Count
        /// </summary>
        private long instances = 0;

        // Sample Physic groups
        private Physic3DCollisionGroup groupA, groupB;

        /// <summary>
        /// Create Scene
        /// </summary>
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;
            RenderManager.DebugLines = true;

            // Createa a Free Camera (W, A, S, D Keyboard controlled)
            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 5, 5), Vector3.Zero);
            EntityManager.Add(camera.Entity);
            RenderManager.SetActiveCamera(camera.Entity);

            // We set group before entities grouping
            this.groupA = new Physic3DCollisionGroup();
            this.groupB = new Physic3DCollisionGroup();

            // GroupA RED will Ignore Self Group Collision
            this.groupA.IgnoreCollisionWith(this.groupA);
            
            // GroupA RED will Ignore GroupB BLUE Collision
            this.groupA.IgnoreCollisionWith(this.groupB);

            // GroupB BLUE will Collide Self group entities. 
            // Every entity collide all groups by default for each new Collision group.
            // Use Physic3DCollisionGroup.DefaulCollisionGroup Property to get Engine Physic3D default Collision group
            
            // Creates a plane ground
            this.CreateGround();

            // Creates Blue-Blue enviroment
            this.CreateSphere(new Vector3(-2, 0, -3), true);
            this.CreateSphere(new Vector3(2, 0, -3), true);

            // Creates Blue-Red enviroment
            this.CreateSphere(new Vector3(-2, 0, -1), true);
            this.CreateSphere(new Vector3(2, 0, -1), false);

            // Creates Red-Red enviroment
            this.CreateSphere(new Vector3(-2, 0, 1), false);
            this.CreateSphere(new Vector3(2, 0, 1), false);
        }

        /// <summary>
        /// Creates a Box Ground
        /// </summary>
        private void CreateGround()
        {
            Vector3 rotation = new Vector3(0, 0, MathHelper.ToRadians(10));
            //Vector3 rotation = Vector3.Zero;

            // Left plane ramp
            Entity groundA = new Entity("groundA")
                .AddComponent(new Transform3D() { Position = new Vector3(-3, -1, 0), Rotation = -rotation, Scale = new Vector3(7, 1, 20) })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
                .AddComponent(new ModelRenderer());
            EntityManager.Add(groundA);

            // Right plane ramp
            Entity groundB = new Entity("groundB")
                .AddComponent(new Transform3D() { Position = new Vector3(3, -1, 0), Rotation = rotation, Scale = new Vector3(7, 1, 20) })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = true })
                .AddComponent(new MaterialsMap(new BasicMaterial(Color.Gray)))
                .AddComponent(new ModelRenderer());
            EntityManager.Add(groundB);
        }

        /// <summary>
        /// Create Sphere
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        private void CreateSphere(Vector3 position, bool isBlueBall)
        {
            Entity primitive = new Entity("Sphere" + instances++)
                .AddComponent(new Transform3D() { Position = position, Scale = Vector3.One / 2 })
                .AddComponent(new SphereCollider())
                .AddComponent(Model.CreateSphere())
                .AddComponent(new RigidBody3D())
                .AddComponent(new MaterialsMap(new BasicMaterial(isBlueBall ? Color.Red : Color.Blue)))
                .AddComponent(new ModelRenderer());
            EntityManager.Add(primitive);

            // Collision Grouping. Blue ball is groupA. Red Ball is groupB
            RigidBody3D rigidBody = primitive.FindComponentOfType<RigidBody3D>();
            if (rigidBody != null)
            {
                rigidBody.CollisionGroup = isBlueBall ? groupA : groupB;
            }
        }
    }
}
