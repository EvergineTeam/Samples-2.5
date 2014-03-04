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
// MouseBehavior
//
// Copyright © 2013 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace OnPhysics2DCollisionSampleProject
{
    /// <summary>
    /// Mouse Behavior
    /// </summary>
    public class MouseBehavior : SceneBehavior
    {
        // input variables
        private Input input;
        private MouseState mouseState;

        // Physic components
        private FixedMouseJoint2D mouseJoint;
        private Entity connectedEntity;

        // Mouse position optimization
        private Vector2 mousePosition = Vector2.Zero;

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

            if (this.input.MouseState.IsConnected)
            {
                this.mouseState = this.input.MouseState;

                // Checks Mouse Left Button Click and anyone entity linked
                if (this.mouseState.LeftButton == ButtonState.Pressed && this.mouseJoint == null)
                {
                    // Udpates Mouse Position
                    this.mousePosition.X = this.mouseState.X;
                    this.mousePosition.Y = this.mouseState.Y;

                    foreach (Entity entity in this.Scene.EntityManager.EntityGraph)
                    {
                        Collider2D collider = entity.FindComponentOfType<Collider2D>();
                        if (collider != null)
                        {
                            // Collider Test
                            if (collider.Contain(mousePosition))
                            {
                                RigidBody2D rigidBody = entity.FindComponentOfType<RigidBody2D>();
                                if (rigidBody != null)
                                {
                                    // Forbiden Mouse Joint of Kinematic Bodies
                                    if (!rigidBody.IsKinematic)
                                    {
                                        this.connectedEntity = entity;

                                        // Create Mouse Joint
                                        this.mouseJoint = new FixedMouseJoint2D(this.mousePosition);
                                        this.connectedEntity.AddComponent(this.mouseJoint);

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
                if (this.mouseState.LeftButton == ButtonState.Release && this.mouseJoint != null)
                {
                    // Remove Fixed Joint
                    this.connectedEntity.RemoveComponent<FixedMouseJoint2D>();
                    this.mouseJoint = null;
                }

                // If joint exists then update joint anchor position
                if (this.mouseJoint != null)
                {
                    this.mousePosition.X = this.mouseState.X;
                    this.mousePosition.Y = this.mouseState.Y;
                    this.mouseJoint.WorldAnchor = this.mousePosition;
                }
            }
        }

        protected override void ResolveDependencies()
        {
        }
    }
}
