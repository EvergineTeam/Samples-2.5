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
#endregion

namespace Motor3DSampleProject
{
    public class MainScene : Scene
    {
        private static long instance;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;
         
            // Main Camera definition and Pick Entity Behavior
            FreeCamera camera = new FreeCamera("MainCamera", new Vector3(0, 40, 50), Vector3.Zero);
            camera.Entity.AddComponent(new PickingBehavior());
            this.EntityManager.Add(camera.Entity);

            // Sample needs Collision groups to ignore collisions between wheels and body
            // Collision group 1: Body
            // Collision group 2: Wheels (ignores collision with body)
            // collision group 3: terrain (collides with body and wheels)
            Physic3DCollisionGroup bodyCollisiongroup = new Physic3DCollisionGroup();

            Physic3DCollisionGroup wheelCollistionGroup = new Physic3DCollisionGroup();
            wheelCollistionGroup.IgnoreCollisionWith(bodyCollisiongroup);

            Physic3DCollisionGroup terrainCollisiongroup = new Physic3DCollisionGroup();
            terrainCollisiongroup.DefineCollisionWith(bodyCollisiongroup);
            terrainCollisiongroup.DefineCollisionWith(wheelCollistionGroup);

            // Creates Entities
            this.CreateGround(new Vector3(0, -5, 0), terrainCollisiongroup);
            this.CreateVehicle(bodyCollisiongroup, wheelCollistionGroup);
        }

        /// <summary>
        /// Creates a Vehicle
        /// </summary>
        /// <param name="bodyCollisiongroup">Vehicle body collision group</param>
        /// <param name="wheelCollisionGroup">Vehicle Wheels collision group</param>
        private void CreateVehicle(Physic3DCollisionGroup bodyCollisiongroup, Physic3DCollisionGroup wheelCollisionGroup)
        {
            bool isKinetikVehicle = false;

            // Initial parts positions
            Vector3 bodyPosition = new Vector3(0, 5, 0);
            Vector3 wheel1Position = bodyPosition + new Vector3( 1, -0.3f,  0.7f);
            Vector3 wheel2Position = bodyPosition + new Vector3( 1, -0.3f, -0.7f);
            Vector3 wheel3Position = bodyPosition + new Vector3(-1, -0.3f,  0.7f);
            Vector3 wheel4Position = bodyPosition + new Vector3(-1, -0.3f, -0.7f);

            // Vehicle Parts entities
            Entity vBody = this.CreateBox(bodyPosition, bodyCollisiongroup, isKinetikVehicle);
            Entity wheel1 = this.CreateCylinder(wheel1Position, isKinetikVehicle, wheelCollisionGroup);
            Entity wheel2 = this.CreateCylinder(wheel2Position, isKinetikVehicle, wheelCollisionGroup);
            Entity wheel3 = this.CreateCylinder(wheel3Position, isKinetikVehicle, wheelCollisionGroup);
            Entity wheel4 = this.CreateCylinder(wheel4Position, isKinetikVehicle, wheelCollisionGroup);

            // Adds to scene
            EntityManager.Add(vBody);
            EntityManager.Add(wheel1);
            EntityManager.Add(wheel2);
            EntityManager.Add(wheel3);
            EntityManager.Add(wheel4);

            // Create Joints and define Motors
            wheel1.AddComponent(new HingeJoint(vBody, wheel1Position, Vector3.Forward) { IsMotorEnabled = true, MotorVelocity = MathHelper.ToRadians(-720), MotorDamping = 1/500, MotorMaxForce = 500 });
            wheel2.AddComponent(new HingeJoint(vBody, wheel2Position, Vector3.Forward) { IsMotorEnabled = true, MotorVelocity = MathHelper.ToRadians(-720), MotorDamping = 1/500, MotorMaxForce = 500 });
            wheel3.AddComponent(new HingeJoint(vBody, wheel3Position, Vector3.Forward));
            wheel4.AddComponent(new HingeJoint(vBody, wheel4Position, Vector3.Forward));
        }

        /// <summary>
        /// Creates a Physical Box
        /// </summary>
        /// <param name="position">Entity Position</param>
        /// <param name="collisionGroup">Collision Group</param>
        /// <param name="isKinematic">Is Kinematic</param>
        /// <returns>A Box Entity</returns>
        private Entity CreateBox(Vector3 position, Physic3DCollisionGroup collisionGroup, bool isKinematic)
        {
            Entity primitive = new Entity("box" + instance++)
                .AddComponent(new Transform3D() { Position = position, Scale = new Vector3(3, 0.5f, 2)})
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = isKinematic, CollisionGroup = collisionGroup })
                .AddComponent(new MaterialsMap())
                .AddComponent(new ModelRenderer());

            return primitive;
        }

        /// <summary>
        /// Creates a Cylindric Entity
        /// </summary>
        /// <param name="position">Entity Position</param>
        /// <param name="isKinematic">Is Kinematic</param>
        /// <param name="collisionGroup">Collision group</param>
        /// <returns>A Cylindical entity</returns>
        private Entity CreateCylinder(Vector3 position, bool isKinematic, Physic3DCollisionGroup collisionGroup)
        {
            Entity primitive = new Entity("cylinder" + instance++)
                .AddComponent(new Transform3D() { Position = position, Scale = new Vector3(1, 0.5f, 1), Rotation = new Vector3(MathHelper.ToRadians(90), 0, 0) })
                .AddComponent(new SphereCollider()) // We use SphereCollider with Cylinder Model.
                .AddComponent(Model.CreateCylinder())
                .AddComponent(new RigidBody3D() { IsKinematic = isKinematic, CollisionGroup = collisionGroup, KineticFriction = 10, StaticFriction = 10 })
                .AddComponent(new MaterialsMap())
                .AddComponent(new ModelRenderer());

            return primitive;
        }

        /// <summary>
        /// Creates a Physical Ground
        /// </summary>
        /// <param name="position">Ground position</param>
        /// <param name="collisionGroup">Ground Collision Group</param>
        private void CreateGround(Vector3 position, Physic3DCollisionGroup collisionGroup)
        {
            Entity terrain = new Entity("terrain" + instance++)
                .AddComponent(new Transform3D() { Position = position, Scale = new Vector3(4)})
                .AddComponent(new MeshCollider())
                .AddComponent(new Model("Content/terrain.wpk"))
                .AddComponent(new RigidBody3D() { IsKinematic = true, CollisionGroup = collisionGroup, KineticFriction = 10, StaticFriction = 10 })
                .AddComponent(new MaterialsMap())
                .AddComponent(new ModelRenderer());

            EntityManager.Add(terrain);
        }
    }
}
