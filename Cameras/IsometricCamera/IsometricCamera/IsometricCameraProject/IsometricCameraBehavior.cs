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
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Input;
using System.Diagnostics;

namespace IsometricCameraProject
{
    public class IsometricCameraBehavior : Behavior
    {

        /// <summary>
        /// Current camera
        /// </summary>
        [RequiredComponent]
        public Camera3D Camera;

        // Cached input
        private Input input;


        // Speed of the mouse movement
        private float speed;

        // Current keyboard state
        KeyboardState keyboardState;

        // Cached direction movements
        private Vector3 forward;
        private Vector3 right;
        private Vector3 back;
        private Vector3 left;

        public IsometricCameraBehavior()
            : base("IsometricCameraBehavior")
        { }

        protected override void Update(TimeSpan gameTime)
        {
            input = WaveServices.Input;

            if (input.KeyboardState.IsConnected)
            {
                keyboardState = input.KeyboardState;

                if (keyboardState.W == ButtonState.Pressed)
                {
                    MoveCamera(ref forward);
                }
                if (keyboardState.S == ButtonState.Pressed)
                {
                    MoveCamera(ref back);
                }
                if (keyboardState.A == ButtonState.Pressed)
                {
                    MoveCamera(ref left);
                }
                if (keyboardState.D == ButtonState.Pressed)
                {
                    MoveCamera(ref right);
                }
            }
            var rotationMatrix = (Matrix.CreateRotationX(MathHelper.ToRadians(45.0f)) * Matrix.CreateRotationY(MathHelper.ToRadians(30.0f)));
            Vector3 transformedReference = Vector3.Transform(Vector3.Down, rotationMatrix);
            Vector3 cameraLookat = Camera.Position + transformedReference;
            var width = WaveServices.Platform.ScreenWidth / 24;
            var height = WaveServices.Platform.ScreenHeight / 24;

            //camera.Projection = Matrix.CreateOrthographic(width, height, camera.NearPlane, camera.FarPlane);
            Camera.LookAt = cameraLookat;
        }


        private void MoveCamera(ref Vector3 addedVector)
        {
            // Manual inline: position += speed * addedVector;

            Camera.Position = new Vector3()
            {
                X = Camera.Position.X + (speed*addedVector.X),
                Y = Camera.Position.Y + (speed*addedVector.Y),
                Z = Camera.Position.Z + (speed*addedVector.Z),
            };
        }


        protected override void Initialize()
        {
            var rotationMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(45.0f)) * Matrix.CreateRotationY(MathHelper.ToRadians(30.0f));
            Vector3 transformedReference = Vector3.Transform(Vector3.Down, rotationMatrix);
            Vector3 cameraLookat = Camera.Position + transformedReference;
            Camera.LookAt = cameraLookat;

            speed = .3f;

            forward = -Vector3.UnitZ;
            right = Vector3.UnitX;
            left = -Vector3.UnitX;
            back = Vector3.UnitZ;
        }
    }
}
