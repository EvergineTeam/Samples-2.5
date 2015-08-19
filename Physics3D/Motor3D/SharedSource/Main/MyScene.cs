#region Using Statements
using System;
using System.Linq;
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

namespace Motor3D
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            this.Load(WaveContent.Scenes.MyScene);

            // This sample needs Collision Groups to ignore such between chasis and wheels
            var chasisCollisionGroup = new Physic3DCollisionGroup();

            var wheelCollisionGroup = new Physic3DCollisionGroup();
            wheelCollisionGroup.IgnoreCollisionWith(chasisCollisionGroup);

            var groundCollisionGroup = new Physic3DCollisionGroup();
            groundCollisionGroup.DefineCollisionWith(chasisCollisionGroup);
            groundCollisionGroup.DefineCollisionWith(wheelCollisionGroup);

            this.SetUpPhysicsOnGround(chasisCollisionGroup);
            this.SetUpPhysicsOnWheels(wheelCollisionGroup);
            this.SetUpPhysicsOnChasis(groundCollisionGroup);
        }

        private void SetUpPhysicsOnChasis(Physic3DCollisionGroup groundCollisionGroup)
        {
            var ground = this.EntityManager.Find("ground");

            if (ground == null)
            {
                return;
            }

            var groundRigidBody = ground.FindComponent<RigidBody3D>();

            if (groundRigidBody == null)
            {
                return;
            }

            groundRigidBody.CollisionGroup = groundCollisionGroup;
        }

        private void SetUpPhysicsOnWheels(Physic3DCollisionGroup wheelCollisionGroup)
        {
            var wheels = this.EntityManager.FindAllByTag("wheel").Cast<Entity>();

            if (wheels == null || wheels.Count() == 0)
            {
                return;
            }

            var chasis = this.EntityManager.Find("chasis");

            if (chasis == null)
            {
                return;
            }

            var i = 0;

            foreach (var wheel in wheels)
            {
                var wheelRigidBody = wheel.FindComponent<RigidBody3D>();

                if (wheelRigidBody == null)
                {
                    return;
                }

                wheelRigidBody.CollisionGroup = wheelCollisionGroup;

                var hingeJoint = new HingeJoint3D(chasis, Vector3.Zero, -Vector3.UnitY);
                var jointMap = new JointMap3D()
                    .AddJoint("chasisWheelJoint" + i++, hingeJoint);

                if (wheel.Name.Contains("Front"))
                {
                    hingeJoint.IsMotorEnabled = true;
                    hingeJoint.MotorVelocity = MathHelper.Pi * 2;
                    hingeJoint.MotorDamping = 1 / 500;
                    hingeJoint.MotorMaxForce = 500;
                }

                wheel.AddComponent(jointMap);
            }
        }

        private void SetUpPhysicsOnGround(Physic3DCollisionGroup chasisCollisionGroup)
        {
            var chasis = this.EntityManager.Find("chasis");

            if (chasis == null)
            {
                return;
            }

            var chasisRigidBody = chasis.FindComponent<RigidBody3D>();

            if (chasisRigidBody == null)
            {
                return;
            }

            chasisRigidBody.CollisionGroup = chasisCollisionGroup;
        }
    }
}
