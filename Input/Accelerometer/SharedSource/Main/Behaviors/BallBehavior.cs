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
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

namespace AccelerometerProject.Behaviors
{
    [DataContract]
    public class BallBehavior : Behavior
    {
        [RequiredComponent()]
        public Transform3D Transform;

        [RequiredComponent]
        public MaterialComponent material;

        private Vector3 input;
        private float seconds;

        protected override void Update(TimeSpan gameTime)
        {
            this.seconds = ((float)gameTime.Milliseconds / 10);

            if (WaveServices.Input.AccelerometerState.IsConnected)
            {
                this.input = WaveServices.Input.AccelerometerState.RawAcceleration * (this.seconds * 10);
            }
            else
            {
                this.input.X = WaveServices.Input.KeyboardState.A == ButtonState.Pressed ? 1.0f :
                    WaveServices.Input.KeyboardState.Z == ButtonState.Pressed ? -1.0f : 0f;
                this.input.Y = WaveServices.Input.KeyboardState.S == ButtonState.Pressed ? 2.0f :
                    WaveServices.Input.KeyboardState.X == ButtonState.Pressed ? -1.0f : 0f;
                this.input.Z = WaveServices.Input.KeyboardState.D == ButtonState.Pressed ? 1.0f :
                    WaveServices.Input.KeyboardState.C == ButtonState.Pressed ? -1.0f : 0f;
            }
        }

        protected override void Initialize()
        {
            var random = WaveServices.Random;

            if (!WaveServices.Platform.IsEditor)
            {
                this.input = new Vector3();

                Vector3 newPosition = new Vector3();

                newPosition.X = random.Next(-100, 100);
                newPosition.Z = random.Next(-100, 100);
                newPosition.Y = random.Next(-100, 100);

                Transform.Position = newPosition;
                this.seconds = 0f;
            }

            if (material.Material != null)
            {
                (material.Material as ForwardMaterial).DiffuseColor = new Color(
                    (float)random.NextDouble(), 
                    (float)random.NextDouble(), 
                    (float)random.NextDouble(), 
                    1f);
            }
        }
    }
}
