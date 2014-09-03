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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Math;
using WaveEngine.Common.Input;

namespace AccelerometerProject.Behaviors
{
    public class CameraBehavior: Behavior
    {
        [RequiredComponent()]
        public Camera3D Camera;
        private Vector3 input;

        internal CameraBehavior()
            : base("CameraBehavior")
        { }

        protected override void Update(TimeSpan gameTime)
        {
            if (WaveServices.Input.AccelerometerState.IsConnected)
            {
                input = WaveServices.Input.AccelerometerState.SmoothAcceleration * ((float)gameTime.Milliseconds * 10);

                Camera.LookAt = input;
            }
            else
            {
                input.X = WaveServices.Input.KeyboardState.A == ButtonState.Pressed ? 1.0f :
                   WaveServices.Input.KeyboardState.Z == ButtonState.Pressed ? -1.0f : 0f;
                input.Y = WaveServices.Input.KeyboardState.S == ButtonState.Pressed ? 2.0f :
                    WaveServices.Input.KeyboardState.X == ButtonState.Pressed ? -1.0f : 0f;
                input.Z = WaveServices.Input.KeyboardState.D == ButtonState.Pressed ? 1.0f :
                    WaveServices.Input.KeyboardState.C == ButtonState.Pressed ? -1.0f : 0f;

                Camera.Position += input;
            }
        }

        protected override void Initialize()
        {
            input = Vector3.Zero;
        }
    }
}
