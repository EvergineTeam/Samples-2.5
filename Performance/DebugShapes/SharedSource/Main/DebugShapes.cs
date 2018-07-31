// Copyright (C) 2012-2015 Weekend Game Studio
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
// DrawableLines
//
// Copyright © 2015 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;

#endregion

namespace DebugShapesProject
{
    [DataContract]
    public class DebugShapes : Drawable3D
    {
        // The shapes that we'll be drawing
        private BoundingOrientedBox box;
        private BoundingFrustum frustum;
        private BoundingSphere sphere;

        public DebugShapes()
            : base("DebugShapes")
        {
        }

        public override void Draw(TimeSpan gameTime)
        {
            // Add our shapes to be rendered
            this.RenderManager.LineBatch3D.DrawBoundingOrientedBox(this.box, Color.Yellow);
            this.RenderManager.LineBatch3D.DrawBoundingFrustum(this.frustum, Color.Green);
            this.RenderManager.LineBatch3D.DrawBoundingSphere(this.sphere, Color.Red);

            //Also add a triangle and a line
            this.RenderManager.LineBatch3D.DrawBox(new Vector3(-2), new Vector3(2), Color.White);
            this.RenderManager.LineBatch3D.DrawCircle(Vector3.Zero, 1.5f, Color.Pink);
            this.RenderManager.LineBatch3D.DrawTriangle(new Vector3(-1f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(0f, 2f, 0f), Color.Purple);
            this.RenderManager.LineBatch3D.DrawLine(new Vector3(0f, 0f, 0f), new Vector3(3f, 3f, 3f), Color.Blue);
        }

        protected override void Initialize()
        {
            // Create a box that is centered on the origin and extends from (-3, -3, -3) to (3, 3, 3)
            this.box = new BoundingOrientedBox(Vector3.Zero, new Vector3(3), Quaternion.Identity);

            // Create our frustum to simulate a camera sitting at the origin, looking down the X axis, with a 16x9
            // aspect ratio, a near plane of 1, and a far plane of 5
            Matrix frustumView = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitX, Vector3.Up);
            Matrix frustumProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 16f / 9f, 1f, 5f);
            this.frustum = new BoundingFrustum(frustumView * frustumProjection);

            // Create a sphere that is centered on the origin and has a radius of 3
            this.sphere = new BoundingSphere(Vector3.Zero, 3f);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}