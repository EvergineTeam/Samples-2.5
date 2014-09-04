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
// ForceBehavior
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

namespace Force2DSampleProject
{
    public class ForceBehavior : Behavior
    {
        private Input input;
        private KeyboardState keyboardState;

        private float multiplicator = 2f;
        private float angularImpulse = 0.1f;
        private float torque = 1f;
        private Vector2 topDirection;
        private Vector2 leftDirection;

        public ForceBehavior(string name) : base(name)
        {
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;
            if (this.input.KeyboardState.IsConnected)
            {
                this.keyboardState = this.input.KeyboardState;

                RigidBody2D rigidBody = Owner.FindComponent<RigidBody2D>();
                if (rigidBody != null)
                {
                    // W, A, S, D applies Directional Linear Impulses
                    Vector2 result = Vector2.Zero;
                    if (keyboardState.A == ButtonState.Pressed)
                    {
                        result += leftDirection;
                    }
                    if (keyboardState.D == ButtonState.Pressed)
                    {
                        result -= leftDirection;
                    }
                    if (keyboardState.W == ButtonState.Pressed)
                    {
                        result += topDirection;
                    }
                    if (keyboardState.S == ButtonState.Pressed)
                    {
                        result -= topDirection;
                    }
                 
                    // Apply Linear Impulse
                    if (result != Vector2.Zero)
                    {
                        rigidBody.ApplyLinearImpulse(result);
                    }

                    // Left and Right arrow applies angular impulse
                    if (keyboardState.Left == ButtonState.Pressed)
                    {
                        rigidBody.ApplyAngularImpulse(-angularImpulse);
                    }
                    else if (keyboardState.Right == ButtonState.Pressed)
                    {
                        rigidBody.ApplyAngularImpulse(angularImpulse);
                    }

                    // J and K applies Torque
                    if (keyboardState.J == ButtonState.Pressed)
                    {
                        rigidBody.ApplyTorque(torque);
                    }
                    else if (keyboardState.K == ButtonState.Pressed)
                    {
                        rigidBody.ApplyTorque(-torque);
                    }
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            topDirection = -Vector2.UnitY * multiplicator;
            leftDirection = -Vector2.UnitX * multiplicator;
        }
    }
}
