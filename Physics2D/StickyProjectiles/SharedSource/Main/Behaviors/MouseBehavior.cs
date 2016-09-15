#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace StickyProjectiles.Behaviors
{
    /// <summary>
    /// Mouse Behavior class.
    /// </summary>
    [DataContract]
    public class MouseBehavior : Behavior
    {
        private Input input;
        private TouchPanelState touchState;
        public MouseJoint2D mouseJoint;
        private VirtualScreenManager vsm;

        public Entity ConnectedEntity;
        public Vector2 TouchPosition;
        private KeyboardState lastKeyboardState;

        /// <summary>
        /// Default values method
        /// </summary>
        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.TouchPosition = Vector2.Zero;
        }

        /// <summary>
        /// Resolve dependencies method
        /// </summary>
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.vsm = this.Owner.Scene.VirtualScreenManager;
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime">game time</param>
        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;

            KeyboardState currentKeyboardState = this.input.KeyboardState;
            if (currentKeyboardState.IsConnected)
            {
                if (currentKeyboardState.IsKeyPressed(Keys.O) &&
                   this.lastKeyboardState.IsKeyReleased(Keys.O))
                {
                    this.RenderManager.DebugLines = !this.RenderManager.DebugLines;
                }

                this.lastKeyboardState = currentKeyboardState;
            }

            if (this.input.TouchPanelState.IsConnected)
            {
                this.touchState = this.input.TouchPanelState;

                if (this.touchState.Count > 0 && this.mouseJoint == null)
                {
                    this.TouchPosition = this.touchState[0].Position;
                    this.vsm.ToVirtualPosition(ref this.TouchPosition);

                    foreach (Entity entity in this.Owner.Scene.EntityManager.FindAllByTag("Draggable"))
                    {
                        Collider2D collider = entity.FindComponent<Collider2D>(false);
                        if (collider != null)
                        {
                            if (collider.Contain(TouchPosition))
                            {
                                RigidBody2D rigidBody = entity.FindComponent<RigidBody2D>();
                                if (rigidBody != null)
                                {
                                    if (rigidBody.PhysicBodyType == WaveEngine.Common.Physics2D.RigidBodyType2D.Dynamic)
                                    {
                                        this.ConnectedEntity = entity;

                                        //Create Joint
                                        this.mouseJoint = new MouseJoint2D()
                                        {
                                            Target = this.TouchPosition,
                                            //MaxForce = 100,
                                            //DampingRatio = 0.5f,
                                            //FrequencyHz = 2000,
                                        };
                                        this.ConnectedEntity.AddComponent(mouseJoint);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (this.touchState.Count == 0 && this.mouseJoint != null)
                {
                    if (!this.ConnectedEntity.IsDisposed)
                    {
                        this.ConnectedEntity.RemoveComponent(this.mouseJoint);
                    }

                    this.mouseJoint = null;
                }

                if (this.mouseJoint != null)
                {
                    this.TouchPosition = this.touchState[0].Position;
                    this.vsm.ToVirtualPosition(ref this.TouchPosition);
                    this.mouseJoint.Target = this.TouchPosition;                   
                }
            }
        }
    }
}
