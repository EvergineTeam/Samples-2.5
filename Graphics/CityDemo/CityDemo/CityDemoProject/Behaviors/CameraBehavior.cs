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
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;

namespace CityDemoProject.Behaviors
{
    public class CameraBehavior: Behavior
    {
        private Vector3 CENTER = new Vector3(35, 100, 545);
        private float angle = 0;

        public CameraBehavior()
            : base("CameraBehavior")
        {
            
        }

        protected override void Update(TimeSpan gameTime)
        {
            this.angle += 0.0003f * (float)gameTime.TotalMilliseconds;

            if (angle > MathHelper.TwoPi)
            {
                angle -= MathHelper.TwoPi;
            }

            Camera3D camera = this.Owner.FindComponent<Camera3D>();
            Vector3 aux;
            aux.X = CENTER.X + 400 * (float)Math.Sin(angle);
            aux.Y = 200;
            aux.Z = CENTER.Z + 400 * (float)Math.Cos(angle);

            camera.Position = aux;

            camera.LookAt = CENTER;
        }
    }
}
