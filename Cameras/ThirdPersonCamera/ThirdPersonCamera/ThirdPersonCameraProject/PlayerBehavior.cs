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
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Graphics;

namespace ThirdPersonCameraProject
{
    public class PlayerBehavior: Behavior
    {

        public float Speed = 2f;

        [RequiredComponent()]
        public Transform3D PlayerTransform;

        private float timeAmount;

        private Input inputService;

        private Vector3 playerPosition;

        private float zMovement;

        // Cached variables
        private Vector3 dir;
        private Vector3 directionResultVector;

        public PlayerBehavior()
            : base("PlayerBehavior")
        { }

        protected override void Initialize()
        {
            base.Initialize();
            
        }

        protected override void Update(TimeSpan gameTime)
        {
            timeAmount = (float)gameTime.Milliseconds / 1000;

            inputService = WaveServices.Input;
            
            zMovement = 0f;

            if (inputService.KeyboardState.IsConnected)
            {
                if (inputService.KeyboardState.W == ButtonState.Pressed)
                {
                    zMovement = (timeAmount);
                }
                else if (inputService.KeyboardState.S == ButtonState.Pressed)
                {
                    zMovement = -(timeAmount);
                }

                if (inputService.KeyboardState.A == ButtonState.Pressed)
                {
                    PlayerTransform.Rotation.Y = PlayerTransform.Rotation.Y + (timeAmount * Speed);
                }
                else if (inputService.KeyboardState.D == ButtonState.Pressed)
                {
                    PlayerTransform.Rotation.Y = PlayerTransform.Rotation.Y - (timeAmount * Speed);
                }

                playerPosition = PlayerTransform.Position;

                dir = Vector3.UnitX;
                dir.X = dir.X - PlayerTransform.Position.X;
                dir.Y = dir.Y - PlayerTransform.Position.Y;
                dir.Z = dir.Z - PlayerTransform.Position.Z;

                dir.Y = 0;
                dir.Normalize();

                dir.X = (float)Math.Sin(PlayerTransform.Rotation.Y);
                dir.Z = (float)Math.Cos(PlayerTransform.Rotation.Y);
                dir.Normalize();

                zMovement = -zMovement * Speed;

                Vector3.Multiply(ref dir, zMovement, out directionResultVector);

                PlayerTransform.Position.X = PlayerTransform.Position.X + directionResultVector.X;
                PlayerTransform.Position.Y = PlayerTransform.Position.Y + directionResultVector.Y;
                PlayerTransform.Position.Z = PlayerTransform.Position.Z + directionResultVector.Z;
            }
        }
    }
}
