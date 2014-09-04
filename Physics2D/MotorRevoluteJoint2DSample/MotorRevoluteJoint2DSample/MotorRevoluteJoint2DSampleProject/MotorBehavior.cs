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
// MotorBehavior
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
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace MotorRevoluteJoint2DSampleProject
{
    /// <summary>
    /// Motor Behavior Class
    /// </summary>
    public class MotorBehavior : Behavior
    {
        private Vector2 initialPosition = new Vector2(90, 100);

        // Input variables
        private Input input;
        private KeyboardState keyboardState;

        // Motor Speed Increase
        private float motorSpeed = 0.7f;

        private float maxSpeed = 12.0f;

        // Motor Revolute Joint
        [RequiredComponent()]
        private JointMap2D jointMap;

        private RevoluteJoint2D revoluteJoint;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public MotorBehavior(string name)
            : base(name)
        {
            this.revoluteJoint = null;
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();

            this.revoluteJoint = jointMap.Joints["joint"] as RevoluteJoint2D;
        }

        /// <summary>
        /// Update Method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(TimeSpan gameTime)
        {
            this.input = WaveServices.Input;

            if (this.input.KeyboardState.IsConnected)
            {
                this.keyboardState = this.input.KeyboardState;

                if (revoluteJoint != null)
                {
                    // A, D, S Keyboard Control (left, right, stop motor)
                    if (this.keyboardState.A == ButtonState.Pressed)
                    {
                        if (revoluteJoint.MotorSpeed + motorSpeed <= maxSpeed)
                        {
                            revoluteJoint.MotorSpeed += motorSpeed;
                        }
                    }
                    else if (this.keyboardState.D == ButtonState.Pressed)
                    {
                        if (revoluteJoint.MotorSpeed - motorSpeed >= -maxSpeed)
                        {
                            revoluteJoint.MotorSpeed -= motorSpeed;
                        }
                    }
                    else if (this.keyboardState.S == ButtonState.Pressed)
                    {
                        revoluteJoint.MotorSpeed = 0.0f;
                    }
                }
            }
        }
    }
}
