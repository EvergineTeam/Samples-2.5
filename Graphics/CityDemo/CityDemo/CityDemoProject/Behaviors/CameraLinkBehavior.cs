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
    public class CameraLinkBehavior : Behavior
    {
        private Camera link;
        private bool linkX, linkY, linkZ;

        public CameraLinkBehavior(Camera link, bool linkX, bool linkY, bool linkZ)
            : base("LinkBehavior")
        {
            this.link = link;
            this.linkX = linkX;
            this.linkY = linkY;
            this.linkZ = linkZ;
        }

        protected override void Update(TimeSpan gameTime)
        {
            Vector3 linkPos = this.link.Position;
            Vector3 ownerPos = this.Owner.FindComponent<Transform3D>().Position;
            ownerPos.X = (this.linkX) ? linkPos.X : ownerPos.X;
            ownerPos.Y = (this.linkY) ? linkPos.Y : ownerPos.Y;
            ownerPos.Z = (this.linkZ) ? linkPos.Z : ownerPos.Z;
        }
    }
}
