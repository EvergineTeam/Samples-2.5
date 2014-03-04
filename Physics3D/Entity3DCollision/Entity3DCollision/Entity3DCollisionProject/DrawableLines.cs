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
// DrawableLines
//
// Copyright © 2013 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace Entity3DCollisionProject
{
    public class DrawableLines : Drawable3D
    {
        private List<Line> lines;
        private Line l;
        
        public DrawableLines()
            : base("DrawableLines")
        {
            lines = new List<Line>();
        }

        public Line AddNewLine(Vector3 startPosition, Vector3 endDisplacement)
        {
            l = new Line();

            l.StartPoint = startPosition;

            l.EndPoint = new Vector3(startPosition.X + endDisplacement.X,
                                       startPosition.Y + endDisplacement.Y,
                                       startPosition.Z + endDisplacement.Z);

            l.Color = Color.Red;

            lines.Add(l);

            return l;
        }

        public void Remove(Line line)
        {
            this.lines.Remove(line);
        }

        public override void Draw(TimeSpan gameTime)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Line l = lines[i];
                RenderManager.LineBatch3D.DrawLine(ref l);
            }
        }

        protected override void Dispose(bool disposing)
        {
        }

        protected override void DrawBasicUnit(int parameter)
        {
        }
    }
}
