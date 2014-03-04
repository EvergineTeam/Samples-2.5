#region File Description
//-----------------------------------------------------------------------------
// DrawableLines
//
// Copyright © 2012 Weekend Game Studio. All rights reserved.
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

namespace LinesProject
{
    public class DrawableLines : Drawable3D
    {
        private float size = 200;
        private List<Line> lines;
        private Line l;
        private WaveEngine.Framework.Services.Random random;
        private double time;

        public DrawableLines(int numOfLines)
            : base("DrawableLines")//, Layer.Opaque)
        {
            lines = new List<Line>();
            random = WaveServices.Random;

            l = new Line();

            for (int i = 0; i < numOfLines; i++)
            {
                AddNewLine();
            }
        }

        private void AddNewLine()
        {
            l.StartPoint = new Vector3((float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                                        (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                                        (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()));

            l.EndPoint = new Vector3((float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                    (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                    (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()));

            l.Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());

            lines.Add(l);
        }

        public override void Draw(TimeSpan gameTime)
        {
            RenderManager.FindLayer(DefaultLayers.Debug).AddDrawable(0, this);
        }

        protected override void Dispose(bool disposing)
        {
        }
        protected override void DrawBasicUnit(int parameter)
        {
            if (lines.Count < 66000)
            {
                for (int i = 0; i < 2; i++)
                {
                    AddNewLine();
                }
            }

            Labels.Add("Lines", lines.Count.ToString());

            for (int i = 0; i < lines.Count; i++)
            {
                Line l = lines[i];
                RenderManager.LineBatch3D.DrawLine(ref l);
            }
        }
    }
}
