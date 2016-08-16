#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Common.Physics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace StickyProjectiles.Behaviors
{
    [DataContract]
    public class ThrowBehavior : Behavior
    {
        private struct JointData
        {
            public Entity arrow;
            public Entity obstacle;
        }

        private int proyectileInstances;
        private Input input;
        private KeyboardState lastKeyboardState;
        private Entity projectileEntity;

        [RequiredComponent]
        private RevoluteJoint2D revoluteJoint2D = null;

        [RequiredComponent]
        private Transform2D arcTransform = null;

        #region Properties
        [DataMember]
        [RenderPropertyAsAsset(AssetType.Prefab)]
        public string ProjectilePrefab { get; set; }

        [DataMember]
        public float Force { get; set; }

        #endregion

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.Force = 5000;
        }

        /// <summary>
        /// Resolve depedencies
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            this.input = WaveServices.Input;
            if (this.revoluteJoint2D != null)
            {
                this.revoluteJoint2D.EnableMotor = true;
            }
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            KeyboardState currentKeyboardState = this.input.KeyboardState;
            if (currentKeyboardState.IsConnected)
            {
                if (currentKeyboardState.IsKeyPressed(Keys.Q)
                    && lastKeyboardState.IsKeyReleased(Keys.Q))
                {
                    this.ThrowProjectile();
                }

                if (currentKeyboardState.IsKeyPressed(Keys.A))
                {
                    this.revoluteJoint2D.MotorSpeed = 1;
                }
                else if (currentKeyboardState.IsKeyPressed(Keys.D))
                {
                    this.revoluteJoint2D.MotorSpeed = -1;
                }
                else
                {
                    this.revoluteJoint2D.MotorSpeed = 0;
                }

                if (currentKeyboardState.IsKeyPressed(Keys.W))
                {
                    this.Force += 10;
                }
                else if (currentKeyboardState.IsKeyPressed(Keys.S))
                {
                    this.Force -= 10;
                    if (this.Force < 0)
                    {
                        this.Force = 0;
                    }
                }

                if (currentKeyboardState.IsKeyPressed(Keys.O) &&
                    this.lastKeyboardState.IsKeyReleased(Keys.O))
                {
                    this.RenderManager.DebugLines = !this.RenderManager.DebugLines;
                }

                this.lastKeyboardState = currentKeyboardState;
            }
        }

        /// <summary>
        /// Throw Projectile method
        /// </summary>
        private void ThrowProjectile()
        {
            // Create new projectil
            if (!string.IsNullOrEmpty(this.ProjectilePrefab))
            {
                this.projectileEntity = this.EntityManager.Instantiate(this.ProjectilePrefab);
                this.projectileEntity.Name = string.Format("Projectile" + proyectileInstances++);
                this.projectileEntity.IsSerializable = false;
                Transform2D projectileTransform = this.projectileEntity.FindComponent<Transform2D>();
                projectileTransform.LocalPosition = arcTransform.LocalPosition;
                projectileTransform.LocalRotation += arcTransform.LocalRotation;
                projectileTransform.LocalScale = arcTransform.LocalScale;

                this.EntityManager.Add(this.projectileEntity);
            }

            // Throw
            if (this.projectileEntity != null)
            {

                Transform2D projectileTransform = this.projectileEntity.FindComponent<Transform2D>();
                RigidBody2D projectileBody = this.projectileEntity.FindComponent<RigidBody2D>();
                Collider2D collider = this.projectileEntity.FindComponent<Collider2D>(false);
                collider.BeginCollision += this.Collider_BeginCollision;


                Vector2 arrowPointing = Vector2.Transform(Vector2.UnitX * 100, projectileTransform.WorldTransform);
                Vector2 flightDirection = arrowPointing - projectileTransform.Position;
                flightDirection.Normalize();
                projectileBody.AngularDamping = 0;

                Vector2 arrowTailPosition = Vector2.Transform(Vector2.UnitX * -35, projectileTransform.WorldTransform);
                projectileBody.ApplyForce(flightDirection * this.Force, arrowTailPosition);
                //projectileBody.ApplyLinearImpulse(flightDirection * 0.2f, arrowTailPosition);        
            }
        }

        private void Collider_BeginCollision(ICollisionInfo2D contact)
        {
            WaveServices.Dispatcher.RunOnWaveThread(() =>
            {
                var colliderA = contact.ColliderA; // Obstacle
                var colliderB = contact.ColliderB; // Arrow

                if (colliderA != null && colliderB != null && colliderA.CollisionCategories == ColliderCategory2D.Cat2)
                {
                    var physicBodyA = colliderA.RigidBody.UserData as RigidBody2D;
                    var physicBodyB = colliderB.RigidBody.UserData as RigidBody2D;
                    //if (physicBodyB.LinearVelocity.Length() > 250.0f)
                    //{
                        Entity obstacle = physicBodyA.Owner;
                        Entity arrow = physicBodyB.Owner;
                        Transform2D obstacleTransform = obstacle.FindComponent<Transform2D>();
                        Transform2D arrowTransform = arrow.FindComponent<Transform2D>();

                        Vector2 arrowHeadLocalPosition = Vector2.UnitX * 178;
                        Vector2 arrowHeadWorldPosition = Vector2.Transform(arrowHeadLocalPosition, arrowTransform.WorldTransform);
                        Vector2 obstacleLocalPosition = Vector2.Transform(arrowHeadWorldPosition, obstacleTransform.WorldInverseTransform);


                        FixedJoint2D distance = new FixedJoint2D()
                        {
                            ConnectedEntityPath = obstacle.EntityPath,
                            Anchor = arrowHeadLocalPosition,
                            ConnectedAnchor = obstacleLocalPosition,
                            ReferenceAngle = obstacleTransform.LocalRotation - arrowTransform.LocalRotation,
                        };
                        arrow.AddComponent(distance);
                    //}
                }
            });
        }
    }
}
