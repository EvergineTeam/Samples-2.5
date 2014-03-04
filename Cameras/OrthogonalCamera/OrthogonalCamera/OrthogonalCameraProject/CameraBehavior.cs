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

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Input;
using WaveEngine.Framework.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components; 
#endregion

namespace OrthogonalCameraProject
{
    public class CameraBehavior : Behavior
    {
        [RequiredComponent]
        public Camera Camera;

        private bool isPerspectiveCamera;
        private bool isPressed;

        public CameraBehavior()
            : base("CameraBehavior")
        {
            Camera = null;
        }

        protected override void Update(TimeSpan gameTime)
        {

            if ((WaveServices.Input.KeyboardState.IsConnected && WaveServices.Input.KeyboardState.O == ButtonState.Pressed)
                ||
                 (WaveServices.Input.GamePadState.IsConnected && WaveServices.Input.GamePadState.Buttons.A == ButtonState.Pressed)
                ||
                (WaveServices.Input.MouseState.IsConnected && WaveServices.Input.MouseState.LeftButton == ButtonState.Pressed)
                ||
                (WaveServices.Input.TouchPanelState.IsConnected && WaveServices.Input.TouchPanelState.Count > 0)
                )
            {
                if (!isPressed)
                {
                    isPressed = true;
                    isPerspectiveCamera = !isPerspectiveCamera;
                    SetCamera();
                }
            }
            else
            {
                isPressed = false;
            }
        }

        protected override void Initialize()
        {
            isPerspectiveCamera = true;
            isPressed = false;
        }

        private void SetCamera()
        {
            if (isPerspectiveCamera)
            {
                setNormalCamera();
            }
            else
            {
                setOrthogonalCamera();
            }

        }

        private void setOrthogonalCamera()
        {
            var width = WaveServices.Platform.ScreenWidth / 8;
            var height = WaveServices.Platform.ScreenHeight / 8;

            Camera.Projection = Matrix.CreateOrthographic(width, height, Camera.NearPlane, Camera.FarPlane);
        }

        private void setNormalCamera()
        {
            Camera.Projection = Matrix.CreatePerspectiveFieldOfView(Camera.FieldOfView, Camera.AspectRatio, Camera.NearPlane, Camera.FarPlane);
        }
    }
}
