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
#endregion

namespace ServoMotor3DSampleProject
{
    public class MyScene : Scene
    {
        /// <summary>
        /// Instance Count
        /// </summary>
        private static long instance;

        /// <summary>
        /// Creates Scene
        /// </summary>
        protected override void CreateScene()
        {
            // Sets MainCamera with pick entity behavior
            FreeCamera mainCamera = new FreeCamera("MainCamera", new Vector3(5, 1, 10), Vector3.Zero)
            {
                BackgroundColor = Color.CornflowerBlue,
            };
            mainCamera.Entity.AddComponent(new PickingBehavior());
            this.EntityManager.Add(mainCamera.Entity);

            this.CreateGround(new Vector3(0, -2.5f, 0));
            this.CreateBed();

            WaveServices.TimerFactory.CreateTimer("MainFallingTimer", TimeSpan.FromSeconds(2), () =>
            {
                this.CreateFalling(new Vector3(2, 5, 2));
            });
        }

        /// <summary>
        /// Creates a Vehicle
        /// </summary>
        private void CreateBed()
        {
            bool isKinetikBed = false;

            float baseToBodyDistance = 1.3f;
            float baseFrontRearDistance = 2f;
            float baseRightLeftDistance = 2.5f;

            // Initial parts positions
            Vector3 bodyPosition = new Vector3(0, 0, 0);
            Vector3 base1Position = bodyPosition + new Vector3(baseRightLeftDistance, -baseToBodyDistance, baseFrontRearDistance);
            Vector3 base2Position = bodyPosition + new Vector3(baseRightLeftDistance, -baseToBodyDistance, -baseFrontRearDistance);
            Vector3 base3Position = bodyPosition + new Vector3(-baseRightLeftDistance, -baseToBodyDistance, baseFrontRearDistance);
            Vector3 base4Position = bodyPosition + new Vector3(-baseRightLeftDistance, -baseToBodyDistance, -baseFrontRearDistance);

            // Bed Parts entities
            Entity vBody = this.CreateBox(bodyPosition, isKinetikBed);
            Entity base1 = this.CreateBase(base1Position, isKinetikBed);
            Entity base2 = this.CreateBase(base2Position, isKinetikBed);
            Entity base3 = this.CreateBase(base3Position, isKinetikBed);
            Entity base4 = this.CreateBase(base4Position, isKinetikBed);

            // Adds to scene
            EntityManager.Add(vBody);
            EntityManager.Add(base1);
            EntityManager.Add(base2);
            EntityManager.Add(base3);
            EntityManager.Add(base4);

            // Create Joints and define Motors
            bool isMotorEnabled = true;
            float motorMaxForce = float.MaxValue;
            float motorDamping = 0;
            float motorStiffness = 100;
            float motorGoalDistance = 2;

            base1.AddComponent(new JointMap3D().AddJoint("jointBase1", new LineSliderJoint(vBody, base1Position, Vector3.Up, base1Position + Vector3.UnitY * baseToBodyDistance) { IsMotorEnabled = isMotorEnabled, MotorMaxForce = motorMaxForce, MotorDamping = motorDamping, MotorStiffness = motorStiffness, MotorGoalDistance = motorGoalDistance }));
            base2.AddComponent(new JointMap3D().AddJoint("jointBase1",new LineSliderJoint(vBody, base2Position, Vector3.Up, base2Position + Vector3.UnitY * baseToBodyDistance) { IsMotorEnabled = isMotorEnabled, MotorMaxForce = motorMaxForce, MotorDamping = motorDamping, MotorStiffness = motorStiffness, MotorGoalDistance = motorGoalDistance }));
            base3.AddComponent(new JointMap3D().AddJoint("jointBase1",new LineSliderJoint(vBody, base3Position, Vector3.Up, base3Position + Vector3.UnitY * baseToBodyDistance) { IsMotorEnabled = isMotorEnabled, MotorMaxForce = motorMaxForce, MotorDamping = motorDamping, MotorStiffness = motorStiffness, MotorGoalDistance = motorGoalDistance }));
            base4.AddComponent(new JointMap3D().AddJoint("jointBase1",new LineSliderJoint(vBody, base4Position, Vector3.Up, base4Position + Vector3.UnitY * baseToBodyDistance) { IsMotorEnabled = isMotorEnabled, MotorMaxForce = motorMaxForce, MotorDamping = motorDamping, MotorStiffness = motorStiffness, MotorGoalDistance = motorGoalDistance }));
        }

        /// <summary>
        /// Creates a Physical Box
        /// </summary>s
        /// <param name="position">Entity Position</param>
        /// <param name="isKinematic">Is Kinematic</param>
        /// <returns>A Box Entity</returns>
        private Entity CreateBox(Vector3 position, bool isKinematic)
        {
            Entity primitive = new Entity("box" + instance++)
                .AddComponent(new Transform3D() { Position = position, Scale = new Vector3(5, 0.2f, 5) })
                .AddComponent(new BoxCollider())
                .AddComponent(Model.CreateCube())
                .AddComponent(new RigidBody3D() { IsKinematic = isKinematic })
                .AddComponent(new MaterialsMap())
                .AddComponent(new ModelRenderer());

            return primitive;
        }

        /// <summary>
        /// Creates a base Entity
        /// </summary>
        /// <param name="position">Entity Position</param>
        /// <param name="isKinematic">Is Kinematic</param>
        /// <returns>A base entity</returns>
        private Entity CreateBase(Vector3 position, bool isKinematic)
        {
            Entity primitive = new Entity("base" + instance++)
                .AddComponent(new Transform3D() { Position = position, Rotation = new Vector3(MathHelper.ToRadians(90), 0, 0) })
                .AddComponent(new SphereCollider())
                .AddComponent(Model.CreateSphere())
                .AddComponent(new RigidBody3D() { IsKinematic = isKinematic })
                .AddComponent(new MaterialsMap())
                .AddComponent(new ModelRenderer());

            return primitive;
        }

        /// <summary>
        /// Creates Ground
        /// </summary>
        /// <param name="position">Ground Position</param>
        private void CreateGround(Vector3 position)
        {
            Entity primitive = new Entity("ground" + instance++)
               .AddComponent(new Transform3D() { Position = position })
               .AddComponent(new BoxCollider())
               .AddComponent(Model.CreatePlane(Vector3.Up, 80))
               .AddComponent(new RigidBody3D() { IsKinematic = true })
               .AddComponent(new MaterialsMap())
               .AddComponent(new ModelRenderer());

            EntityManager.Add(primitive);
        }

        /// <summary>
        /// Create Falling object
        /// </summary>
        /// <param name="position">Falling object position</param>
        private void CreateFalling(Vector3 position)
        {
            Entity primitive = new Entity("fallingCrate" + instance++)
               .AddComponent(new Transform3D() { Position = position })
               .AddComponent(new SphereCollider())
               .AddComponent(Model.CreateSphere())
               .AddComponent(new RigidBody3D())
               .AddComponent(new MaterialsMap())
               .AddComponent(new ModelRenderer());

            EntityManager.Add(primitive);

            // Sets sphere Time To Live. Remove timer from WaveServices after use.
            WaveServices.TimerFactory.CreateTimer("Timer" + primitive.Name, TimeSpan.FromSeconds(5), () =>
            {
                EntityManager.Remove(primitive);
                WaveServices.TimerFactory.RemoveTimer("Timer" + primitive.Name);
            });
        }
    }
}
