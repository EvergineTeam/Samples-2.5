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
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace AccelerometerProject.Behaviors
{
    public class BallBehavior : Behavior
    {
        [RequiredComponent()]
        public Transform3D Transform;
        private Vector3 input;
        private float seconds;
        private float weight;

        //private RigidBody body;

        public BallBehavior(string name)
            : base(name)
        { }

        protected override void Update(TimeSpan gameTime)
        {
            seconds = ((float)gameTime.Milliseconds / 10);

            if (WaveServices.Input.AccelerometerState.IsConnected)
                input = WaveServices.Input.AccelerometerState.RawAcceleration * (seconds*10);
            else
            {
                input.X = WaveServices.Input.KeyboardState.A == ButtonState.Pressed ? 1.0f :
                    WaveServices.Input.KeyboardState.Z == ButtonState.Pressed ? -1.0f : 0f;
                input.Y = WaveServices.Input.KeyboardState.S == ButtonState.Pressed ? 2.0f :
                    WaveServices.Input.KeyboardState.X == ButtonState.Pressed ? -1.0f : 0f;
                input.Z = WaveServices.Input.KeyboardState.D == ButtonState.Pressed ? 1.0f :
                    WaveServices.Input.KeyboardState.C == ButtonState.Pressed ? -1.0f : 0f;
            }

            
            //transform.Position += input*seconds * weight;
            //transform.Position.Y -= .98f * (seconds);
            
            
            //if (transform.Position.X < -50f) transform.Position.X = -50f;
            //if (transform.Position.X > 50f) transform.Position.X = 50f;
            //if (transform.Position.Z < -30f) transform.Position.Z = -30f;
            //if (transform.Position.Z > 30f) transform.Position.Z = 30f;
            //if (transform.Position.Y < 0) transform.Position.Y = 0f;
        }

        protected override void Initialize()
        {
            input = new Vector3();

            var random = WaveServices.Random;

            Transform.Position.X = random.Next(0, 100);
            Transform.Position.Z = random.Next(0, 100);
            Transform.Position.Y = random.Next(0, 100);

            if (random.NextBool())
                Transform.Position.X *= -1f;

            if (random.NextBool())
                Transform.Position.Z *= -1f;

            if (random.NextBool())
                Transform.Position.Y *= -1f;

            seconds = 0f;
            weight = random.Next(1, 10) / 10f;

            WaveServices.Input.DisplayOrientationState.Orientation = DisplayOrientation.LandscapeLeft;

        }
    }
}
