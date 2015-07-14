using System;
using System.Collections.Generic;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;

namespace OnPhysics2dCollisionEvent
{
    /// <summary>
    /// Mouse Behavior
    /// </summary>
    public class MouseBehavior : SceneBehavior
    {
        // input variables
        private Input input;
        private TouchPanelState touchState;

        // Physic components
        private FixedMouseJoint2D mouseJoint;
        private Entity connectedEntity;

        // Mouse position optimization
        private Vector2 touchPosition = Vector2.Zero;

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseBehavior()
            : base("MouseBehaviour")
        {
        }

        /// <summary>
        /// Update Method
        /// </summary>
        /// <param name="gameTime">Current Game Time</param>
        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;

            if (this.input.TouchPanelState.IsConnected)
            {
                this.touchState = this.input.TouchPanelState;

                // Checks Mouse Left Button Click and anyone entity linked
                if (this.touchState.Count > 0 && this.mouseJoint == null)
                {
                    // Udpates Mouse Position
                    this.touchPosition = this.touchState[0].Position;

                    foreach (Entity entity in this.Scene.EntityManager.EntityGraph)
                    {
                        Collider2D collider = entity.FindComponent<Collider2D>(false);
                        if (collider != null)
                        {
                            // Collider Test
                            if (collider.Contain(touchPosition))
                            {
                                RigidBody2D rigidBody = entity.FindComponent<RigidBody2D>();
                                if (rigidBody != null)
                                {
                                    // Forbiden Mouse Joint of Kinematic Bodies
                                    if (rigidBody.PhysicBodyType != PhysicBodyType.Kinematic)
                                    {
                                        this.connectedEntity = entity;

                                        // Create Mouse Joint
                                        this.mouseJoint = new FixedMouseJoint2D(this.touchPosition);
                                        JointMap2D jointMap = this.connectedEntity.FindComponent<JointMap2D>();
                                        jointMap.AddJoint("mouseJoint", this.mouseJoint);

                                        // We can break after collider test when true, but we'll miss overlapped entities if Physic entity is 
                                        // under a non Physic entity. We are breaking here just for sample.
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                // Checks Mouse Left Button Release
                if (this.touchState.Count == 0 && this.mouseJoint != null)
                {
                    if (!this.connectedEntity.IsDisposed)
                    {
                        // Remove Fixed Joint
                        JointMap2D jointMap2D = this.connectedEntity.FindComponent<JointMap2D>();
                        jointMap2D.RemoveJoint("mouseJoint");
                    }

                    this.mouseJoint = null;
                }

                // If joint exists then update joint anchor position
                if (this.mouseJoint != null)
                {
                    this.touchPosition = this.touchState[0].Position;
                    this.mouseJoint.WorldAnchor = this.touchPosition;
                }
            }
        }

        protected override void ResolveDependencies()
        {
        }
    }
}
